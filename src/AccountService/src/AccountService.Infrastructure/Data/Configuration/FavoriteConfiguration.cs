using AccountService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Data.Configuration;

public static class FavoriteConfiguration
{
    public static void ConfigureFavorite(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("favorites_pkey");

            entity.ToTable("favorites");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Iduser).HasColumnName("iduser");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Volute)
                .HasMaxLength(200)
                .HasColumnName("volute");

            entity.HasOne(d => d.IduserNavigation).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.Iduser)
                .HasConstraintName("favorites_iduser_fkey");
        });
    }
}