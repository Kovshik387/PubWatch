using AuthorizationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationOld.Interfaces;

public interface IDbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}