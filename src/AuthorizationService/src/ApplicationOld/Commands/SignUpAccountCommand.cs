using ApplicationOld.Interfaces;
using ApplicationOld.Dto;
using AuthorizationService.Domain;
using AuthorizationService.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApplicationOld.Commands;

public record SignUpAccountCommand(string Email, string Password, string Device) : IRequest<AuthDto>;

public class SignUpAccountCommandHandler : IRequestHandler<SignUpAccountCommand, AuthDto>
{
    private readonly ILogger<SignInAccountCommandHandler> _logger;
    private readonly IDbContext _dbContext;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IValidator<SignUpAccountCommand> _signUpAccountValidator;
        
    public SignUpAccountCommandHandler(ILogger<SignInAccountCommandHandler> logger, IDbContext dbContext,
        IJwtGenerator jwtGenerator, IValidator<SignUpAccountCommand> signUpAccountValidator)
    {
        _logger = logger;
        _dbContext = dbContext;
        _jwtGenerator = jwtGenerator;
        _signUpAccountValidator = signUpAccountValidator;
    }
    
    public async Task<AuthDto> Handle(SignUpAccountCommand request, CancellationToken cancellationToken)
    {
        var validated = await _signUpAccountValidator.ValidateAsync(request, cancellationToken);
        if (!validated.IsValid)
            throw new ValidationException(validated.Errors);
        
        var accountExist = await _dbContext.Accounts
            .Where(x => x.EmailNormalized == request.Email.ToUpper())
            .FirstOrDefaultAsync(cancellationToken);
        
        if (accountExist is not null) throw new ValidationException("Account already exists");
        
        var accountGuid = Guid.NewGuid();
        var token = _jwtGenerator.GenerateJwtToken(accountGuid);

        var account = new Account
        {
            Email = request.Email,
            EmailNormalized = request.Email.ToUpper(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password,10),
            Refreshes = []
        };
        
        //TODO request to AccountService

        var refresh = new RefreshToken
        {
            Token = token.RefreshToken,
            Device = request.Device,
            Idaccount = accountGuid,
        };

        _dbContext.Accounts.Add(account); _dbContext.RefreshTokens.Add(refresh);        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Account {account.Email} has been successfully created");
        
        return token;
    }
}

