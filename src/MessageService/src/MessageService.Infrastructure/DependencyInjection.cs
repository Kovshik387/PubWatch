using MessageService.Application.Interfaces;
using MessageService.Infrastructure.Services;
using MessageService.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessageService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName != null &&
                        s.FullName.StartsWith("MessageService.", StringComparison.CurrentCultureIgnoreCase));
        
        services.AddAutoMapper(assemblies);

        services.AddTransient<IServiceClient, HttpServiceClient>();
        
        services.Configure<HttpEndPoint>(configuration.GetSection(nameof(HttpEndPoint)));
        
        services.AddDistributedMemoryCache();
        
        return services;
    }
}