using ExchangeService.Application.Interfaces;
using ExchangeService.Domain.Entities;
using ExchangeService.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ExchangeService.Infrastructure.Data;

public class ExchangeDbContext : DbContext, IDbContext
{
    public ExchangeDbContext(DbContextOptions<ExchangeDbContext> options) : base(options) { }
    
    public DbSet<Quotation> Quotations { get; set; }
    public DbSet<Currency> Currencies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ConfigureCurrency();
        modelBuilder.ConfigureQuotation();
    }
}