using ExchangeService.Application.Interfaces;
using ExchangeService.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ExternEndPointRoute>(configuration.GetSection(nameof(ExternEndPointRoute)));
        
        services.AddTransient<IExchangeService, Services.ExchangeService>();
        
        return services;
    }
}