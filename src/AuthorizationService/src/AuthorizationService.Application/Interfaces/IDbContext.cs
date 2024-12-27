using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Application.Interfaces;

public interface IDbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}