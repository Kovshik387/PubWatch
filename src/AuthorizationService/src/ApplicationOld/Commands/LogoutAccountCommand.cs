using ApplicationOld.Interfaces;
using AuthorizationService.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApplicationOld.Commands;

public record LogoutAccountCommand(string RefreshToken) : IRequest<bool>;

public class LogoutAccountCommandHandler : IRequestHandler<LogoutAccountCommand, bool>
{
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IDbContext _dbContext;
    
    public LogoutAccountCommandHandler(IJwtGenerator jwtGenerator, IDbContext dbContext)
    {
        _jwtGenerator = jwtGenerator;
        _dbContext = dbContext;
    }
    
    public async Task<bool> Handle(LogoutAccountCommand request, CancellationToken cancellationToken)
    {
        var accountGuid = _jwtGenerator.GetUserByRefreshToken(request.RefreshToken);
        
        if (accountGuid is null)
            throw new RefreshTokenException("Invalid or expired refresh token");
        
        var account = await _dbContext.Accounts.
            FirstOrDefaultAsync(x => x.Id == Guid.Parse(accountGuid), cancellationToken);
        
        var token = account.Refreshes.FirstOrDefault(x => x.Token == request.RefreshToken);
        
        if (token is null)
            throw new RefreshTokenException("Invalid or expired refresh token");
        
        account.Refreshes.Remove(token); await _dbContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}