using MessagePublisher.Interfaces;
using MessagePublisher.Services;
using MessagePublisher.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessagePublisher;

public static class DependencyInjection
{
    public static IServiceCollection AddMessagePublisher(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection(nameof(RabbitMqSettings)));
        
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        
        return services;
    }
}