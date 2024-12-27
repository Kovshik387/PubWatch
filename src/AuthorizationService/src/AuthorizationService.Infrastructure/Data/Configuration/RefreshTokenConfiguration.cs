using AuthorizationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Infrastructure.Data.Configuration;

public static class RefreshTokenConfiguration
{
    public static void ConfigureRefreshToken(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("refresh_pkey");
            
            entity.ToTable("refreshes");
            
            entity.Property(e => e.Device)
                .HasMaxLength(255)
                .HasColumnName("device");
            
            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            
            entity.Property(e => e.Token)
                .HasColumnName("token");
            
            entity.Property(e => e.Device)
                .HasMaxLength(255)
                .HasColumnName("device");
            
            entity.Property(e => e.Idaccount).HasColumnName("idaccount");
            
            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Refreshes)
                .HasForeignKey(d => d.Idaccount)
                .HasConstraintName("refresh_idaccount_fkey");
            //
            // entity.Property(e => e.Expires)
            //     .HasColumnType("expire");
        });
    }
}