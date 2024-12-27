using AccountService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Data.Configuration;

public static class UserConfiguration
{
    public static void ConfigureUser(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Accept)
                .HasDefaultValue(false)
                .HasColumnName("accept");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(100)
                .HasColumnName("patronymic");
            entity.Property(e => e.Surname)
                .HasMaxLength(100)
                .HasColumnName("surname");
            entity.Property(e => e.IdPhoto)
                .HasMaxLength(255)
                .HasColumnName("id_photo");
        });
    }
}