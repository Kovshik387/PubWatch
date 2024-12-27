using AuthorizationService.Application.Interfaces;
using AuthorizationService.Domain.Entities;
using AuthorizationService.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Infrastructure.Data;

public class AuthorizationDbContext :  DbContext, IDbContext
{
    public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options) {}
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ConfigureAccount();
        modelBuilder.ConfigureRefreshToken();
    }
}