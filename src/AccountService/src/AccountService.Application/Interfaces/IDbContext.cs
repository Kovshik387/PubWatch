using AccountService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Interfaces;

public interface IDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}