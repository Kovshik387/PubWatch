using ApplicationOld.Dto;
using ApplicationOld.Interfaces;
using AuthorizationService.Domain.Entities;
using AuthorizationService.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApplicationOld.Commands;

public record SignInAccountCommand(string Email, string Password, string Device) : IRequest<AuthDto>;

public class SignInAccountCommandHandler : IRequestHandler<SignInAccountCommand, AuthDto>
{
    private readonly ILogger<SignInAccountCommandHandler> _logger;
    private readonly IDbContext _dbContext;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IValidator<SignInAccountCommand> _signInAccountValidator;

    public SignInAccountCommandHandler(IDbContext dbContext,ILogger<SignInAccountCommandHandler> logger,
        IJwtGenerator jwtGenerator, IValidator<SignInAccountCommand> signInAccountValidator)
    {
        _dbContext = dbContext; _logger = logger;
        _jwtGenerator = jwtGenerator;
        _signInAccountValidator = signInAccountValidator;
    }
    
    public async Task<AuthDto> Handle(SignInAccountCommand request, CancellationToken cancellationToken)
    {
        var validated = await _signInAccountValidator.ValidateAsync(request, cancellationToken);
        if (!validated.IsValid)
            throw new ValidationException(validated.Errors);
        
        var account = await _dbContext.Accounts.
            FirstOrDefaultAsync(x => x.Email.Equals(request.Email.ToUpper()), cancellationToken);
        
        if (account is null) throw new NotFoundException("Account not found");
        
        if (!BCrypt.Net.BCrypt.Verify(request.Password,account.PasswordHash))
            throw new UnauthorizedAccessException();
        
        var authResult = _jwtGenerator.GenerateJwtToken(account.Id);
        
        var device = account.Refreshes.
            FirstOrDefault(x => x.Device.Equals(request.Device));

        if (device is null)
        {
            account.Refreshes.Add(new RefreshToken()
            {
                Device = request.Device,
                Token = authResult.AccessToken,
            });
        }
        else
        {
            device.Token = authResult.AccessToken;
        }
        
        _dbContext.Accounts.Update(account); await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Account: {0} has been logged in.",account.Email);
        
        return authResult;
    }
    
}
