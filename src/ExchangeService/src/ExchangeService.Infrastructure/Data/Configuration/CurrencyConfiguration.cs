using ExchangeService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExchangeService.Infrastructure.Data.Configuration;

public static class CurrencyConfiguration
{
    public static void ConfigureCurrency(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("currency_pkey");

            entity.ToTable("currency");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Charcode)
                .HasMaxLength(50)
                .HasColumnName("charcode");
            entity.Property(e => e.Idname)
                .HasMaxLength(255)
                .HasColumnName("idname");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Nominal).HasColumnName("nominal");
            entity.Property(e => e.Numcode).HasColumnName("numcode");
            entity.Property(e => e.Valcursid).HasColumnName("valcursid");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.Vunitrate).HasColumnName("vunitrate");

            entity.HasOne(d => d.Valcurs).WithMany(p => p.Volutes)
                .HasForeignKey(d => d.Valcursid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("currency_quotationid_fkey");
        });
    }
}