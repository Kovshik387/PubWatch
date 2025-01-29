using System.Reflection;
using AccountService.Application.Interfaces;
using AccountService.Infrastructure.Data;
using AccountService.Infrastructure.Services;
using AccountService.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDataBase(services, configuration);

        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName != null && s.FullName.ToLower().StartsWith("accountservice."));
        
        services.AddAutoMapper(assemblies);

        services.AddTransient<IServiceClient, GrpcServiceClient>();
        
        return services;
    }

    private static void AddDataBase(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)));

        var settings = configuration.GetSection(nameof(DbSettings)).Get<DbSettings>();

        if (string.IsNullOrEmpty(settings?.ConnectionString))
        {
            throw new ArgumentNullException(nameof(settings.ConnectionString));
        }
        
        var dbInit = Configure(settings);

        services.AddDbContextFactory<AccountDbContext>(dbInit);
        
        services.AddScoped<IDbContext,AccountDbContext>();
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