using AuthorizationService.Application.Dto;
using AuthorizationService.Application.Interfaces;
using AuthorizationService.Application.Response;
using AuthorizationService.Domain.Entities;
using AuthorizationService.Domain.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthorizationService.Application.Services;

public class AuthService : IAuthService
{
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IDbContext _dbContext;
    private readonly ILogger<AuthService> _logger;
    
    private readonly IServiceClient _serviceClient;
    
    private readonly IValidator<SignInDto> _signInValidator;
    private readonly IValidator<SignUpDto> _signUpValidator;
    
    public AuthService(IJwtGenerator jwtGenerator, IDbContext dbContext, ILogger<AuthService> logger,
        IValidator<SignInDto> signInValidator, IValidator<SignUpDto> signUpValidator,
        IServiceClient serviceClient)
    {
        _jwtGenerator = jwtGenerator; _dbContext = dbContext; _logger = logger;
        _signInValidator = signInValidator; _signUpValidator = signUpValidator;
        _serviceClient = serviceClient;
    }

    public async Task<AuthDto> SignInAsync(SignInDto model)
    {
        var validated = await _signInValidator.ValidateAsync(model);
        if (!validated.IsValid)
            throw new ValidationException(validated.Errors);
        
        var account = await _dbContext.Accounts.
            FirstOrDefaultAsync(x => x.EmailNormalized.Equals(model.Email.ToUpper()));
        
        if (account is null) throw new NotFoundException("Email or password is incorrect");
        
        if (!BCrypt.Net.BCrypt.Verify(model.Password,account.PasswordHash))
            throw new NotFoundException("Email or password is incorrect");
        
        var authResult = _jwtGenerator.GenerateJwtToken(account.Id);
        
        var device = account.Refreshes.
            FirstOrDefault(x => x.Device.Equals(model.Device));

        if (device is null)
        {
            account.Refreshes.Add(new RefreshToken()
            {
                Device = model.Device,
                Token = authResult.AccessToken,
            });
        }
        else
        {
            device.Token = authResult.AccessToken;
        }
        
        _dbContext.Accounts.Update(account); await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Account: {0} has been logged in.",account.Email);
        
        return authResult;
    }

    public async Task<AuthDto> SignUpAsync(SignUpDto model)
    {
        var validated = await _signUpValidator.ValidateAsync(model);
        if (!validated.IsValid)
            throw new ValidationException(validated.Errors);
        
        var accountExist = await _dbContext.Accounts
            .Where(x => x.EmailNormalized.Equals(model.Email.ToUpper()))
            .FirstOrDefaultAsync();
        
        if (accountExist is not null) throw new ValidationException("Account already exists");
        
        var accountGuid = Guid.NewGuid();
        var token = _jwtGenerator.GenerateJwtToken(accountGuid);

        var account = new Account
        {
            Id = accountGuid,
            Email = model.Email,
            EmailNormalized = model.Email.ToUpper(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password,10),
        };
        
        var response = await _serviceClient.Send<AddAccountResponse>(new AccountDto()
        {
            AccountId = account.Id,
            Name = model.Name,
            SurName = model.Surname,
            Patronymic = model.Patronymic,
            Email = account.Email,  
        });

        if (!response.Success)
        {
            return new AuthDto(new Guid(), "", "");
        }
        
        var refresh = new RefreshToken
        {
            Token = token.RefreshToken,
            Device = model.Device ?? "",
            Idaccount = accountGuid,
        };
        
        _dbContext.Accounts.Add(account); _dbContext.RefreshTokens.Add(refresh);        
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation($"Account {account.Email} has been successfully created");
        
        return token;
    }

    public async Task<bool> IsEmptyAsync()
    {
        return !await _dbContext.Accounts.AnyAsync();
    }

    public async Task<AuthDto> GetAccessTokenAsync(RefreshDto request)
    {
        _logger.LogInformation(_jwtGenerator.GetExpireTime(request.RefreshToken));
        
        if (DateTime.UtcNow > DateTimeOffset
                .FromUnixTimeSeconds(long.Parse(_jwtGenerator.GetExpireTime(request.RefreshToken) ?? string.Empty)).UtcDateTime)
            throw new RefreshTokenException("Invalid or expired refresh token");
        
        var idUser = _jwtGenerator.GetUserByRefreshToken(request.RefreshToken);
        
        _logger.LogInformation($"IdUser: {idUser}");
        
        if (idUser is null) throw new RefreshTokenException($"Invalid refresh with user id {idUser} token");

        var user = await _dbContext.Accounts
            .Include(account => account.Refreshes)
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(idUser));

        var tokenExist = user?.Refreshes.FirstOrDefault(x => x.Token.Equals(request.RefreshToken)); 
        _logger.LogInformation($"User: exits: {tokenExist}");
        if (tokenExist is null) throw new RefreshTokenException("Invalid refresh token");
        _logger.LogInformation("Token exist");
        var tokens = _jwtGenerator.GenerateJwtToken(Guid.Parse(idUser));
        _logger.LogInformation("Token generated");
        tokenExist.Token = tokens.RefreshToken; await _dbContext.SaveChangesAsync();

        return tokens;
    }

    public async Task<bool> Logout(string refreshToken)
    {
        var accountGuid = _jwtGenerator.GetUserByRefreshToken(refreshToken);
        
        if (accountGuid is null)
            throw new RefreshTokenException("Invalid or expired refresh token");
        
        var account = await _dbContext.Accounts.
            FirstOrDefaultAsync(x => x.Id == Guid.Parse(accountGuid));
        
        if (account is null) throw new NullReferenceException("Account not found");
        
        var token = account.Refreshes.FirstOrDefault(x => x.Token == refreshToken);
        
        if (token is null)
            throw new RefreshTokenException("Invalid or expired refresh token");
        
        account.Refreshes.Remove(token); await _dbContext.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> LogoutAll(string refreshToken)
    {
        var accountGuid = _jwtGenerator.GetUserByRefreshToken(refreshToken);
        
        if (accountGuid == null) throw new RefreshTokenException("Invalid or expired refresh token");
        
        var account = await _dbContext.Accounts
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(accountGuid));
        
        if (account.Refreshes.FirstOrDefault(x => x.Token == refreshToken) is null)
            throw new RefreshTokenException("Invalid refresh token");
        
        var refreshes = account.Refreshes.Where(x => x.Token != refreshToken);
        foreach (var refresh in refreshes)
            account.Refreshes.Remove(refresh);
        
        _dbContext.Accounts.Update(account);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}