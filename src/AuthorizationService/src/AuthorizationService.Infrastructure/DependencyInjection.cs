using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)));

        var settings = configuration.GetSection(nameof(DbSettings)).Get<DbSettings>();

        if (string.IsNullOrEmpty(settings?.ConnectionString))
        {
            throw new ArgumentNullException(nameof(settings.ConnectionString));
        }
        
        var dbInit = Configure(settings);

        services.AddDbContextFactory<AuthorizationDbContext>(dbInit);
        
        services.AddScoped<IDbContext,AuthorizationDbContext>();
        
        return services;
    }

    private static Action<DbContextOptionsBuilder> Configure(DbSettings dbSettings)
    {
        return (builder) =>
        {
            builder.UseNpgsql(dbSettings.ConnectionString, options => options
                .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                .MigrationsHistoryTable("_migrations")
            );

            if (dbSettings.DetailedLog)
            {
                builder.EnableSensitiveDataLogging();
            }
            
            builder.UseLazyLoadingProxies(true);
        };
    }
}