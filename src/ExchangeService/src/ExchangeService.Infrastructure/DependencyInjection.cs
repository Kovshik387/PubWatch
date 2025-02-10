using ExchangeService.Application.Interfaces;
using ExchangeService.Infrastructure.Data;
using ExchangeService.Infrastructure.Services;
using ExchangeService.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataBase(configuration);
        
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName != null &&
                        s.FullName.StartsWith("ExchangeService.", StringComparison.CurrentCultureIgnoreCase));
        
        services.AddAutoMapper(assemblies);

        services.AddTransient<IHttpServiceClient, HttpServiceClient>();
        
        return services;
    }
    
    private static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)));
        
        var settings = configuration.GetSection(nameof(DbSettings)).Get<DbSettings>();

        if (string.IsNullOrEmpty(settings?.ConnectionString))
        {
            throw new ArgumentNullException(nameof(settings.ConnectionString));
        }
        
        var dbInit = Configure(settings);

        services.AddDbContextFactory<ExchangeDbContext>(dbInit);
        
        services.AddScoped<IDbContext,ExchangeDbContext>();
        
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