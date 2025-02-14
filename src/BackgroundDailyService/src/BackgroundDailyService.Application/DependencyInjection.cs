using BackgroundDailyService.Application.Interfaces;
using BackgroundDailyService.Application.Services;
using BackgroundDailyService.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackgroundDailyService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDailyReceiverService,DailyReceiverService>();
        
        services.Configure<QueueSettings>(configuration.GetSection(nameof(QueueSettings)));
        
        return services;
    }
}