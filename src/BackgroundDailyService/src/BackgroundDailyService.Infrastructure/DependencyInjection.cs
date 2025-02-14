using BackgroundDailyService.Application.Interfaces;
using BackgroundDailyService.Infrastructure.Services;
using BackgroundDailyService.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackgroundDailyService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SecretSettings>(configuration.GetSection(nameof(SecretSettings)));
        services.Configure<EndPoints>(configuration.GetSection(nameof(EndPoints)));

        services.AddScoped<IServiceClient, HttpServiceClient>();
        services.AddScoped<IServiceClient, GrpcServiceClient>();

        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName != null &&
                        s.FullName.StartsWith("BackgroundDailyService.", StringComparison.CurrentCultureIgnoreCase));
        
        services.AddAutoMapper(assemblies);
        
        return services;
    }
}