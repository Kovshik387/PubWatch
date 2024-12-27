using AccountService.Domain.Entities;
using AccountService.Application.Interfaces;
using AccountService.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Data;

public class AccountDbContext : DbContext, IDbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options) { }
    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureUser();
        modelBuilder.ConfigureFavorite();
    }
}