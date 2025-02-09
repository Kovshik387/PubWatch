using ExchangeService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExchangeService.Application.Interfaces;

public interface IDbContext
{
    public DbSet<Quotation> Quotations { get; set; }
    
    public DbSet<Currency> Currencies { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}