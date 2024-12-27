using AuthorizationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Infrastructure.Data.Configuration;

public static class AccountConfiguration
{
    public static void ConfigureAccount(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("auth_pkey");
            
            entity.ToTable("authorization");

            entity.HasIndex(e => e.Email, "auth_email_key").IsUnique();

            entity.HasIndex(e => e.EmailNormalized, "auth_normalized_email_key").IsUnique();
            
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            
            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
        });
    }
}