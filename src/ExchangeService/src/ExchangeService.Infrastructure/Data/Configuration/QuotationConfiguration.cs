using ExchangeService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExchangeService.Infrastructure.Data.Configuration;

public static class QuotationConfiguration
{
    public static void ConfigureQuotation(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quotation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quotation_pkey");

            entity.ToTable("quotation");

            entity.HasIndex(e => e.Date, "date").IsUnique();

            entity.HasIndex(e => e.Date, "quotation_date_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });
    }
}