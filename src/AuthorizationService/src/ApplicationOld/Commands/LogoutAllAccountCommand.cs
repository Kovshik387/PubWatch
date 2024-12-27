using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public record LogoutAllAccountCommand(string RefreshToken) : IRequest<bool>;

public class LogoutAllAccountCommandHandler : IRequestHandler<LogoutAllAccountCommand, bool>
{
    private readonly IDbContext _dbContext;
    private readonly IJwtGenerator _jwtGenerator;
    public LogoutAllAccountCommandHandler(IDbContext dbContext, IJwtGenerator jwtGenerator)
    {
        _dbContext = dbContext;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<bool> Handle(LogoutAllAccountCommand request, CancellationToken cancellationToken)
    {
        var accountGuid = _jwtGenerator.GetUserByRefreshToken(request.RefreshToken);
        
        if (accountGuid == null) throw new RefreshTokenException("Invalid or expired refresh token");
        
        var account = await _dbContext.Accounts
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(accountGuid), cancellationToken);
        
        if (account.Refreshes.FirstOrDefault(x => x.Token == request.RefreshToken) is null)
            throw new RefreshTokenException("Invalid refresh token");
        
        account.Refreshes.Clear(); _dbContext.Accounts.Update(account);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}