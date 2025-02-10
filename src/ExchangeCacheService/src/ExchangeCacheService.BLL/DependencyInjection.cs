using ExchangeCacheService.BLL.Interfaces;
using ExchangeCacheService.BLL.Services;
using ExchangeCacheService.BLL.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeCacheService.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddBllLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisSettings>(configuration.GetSection(nameof(RedisSettings)));
        services.Configure<GrpcEndPointRoute>(configuration.GetSection(nameof(GrpcEndPointRoute)));

        services.AddTransient<ICacheService, CacheService>();

        var redisSettings = configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>();
        
        if (redisSettings is null) throw new ArgumentException("Redis settings not found");
        
        services.AddStackExchangeRedisCache(x =>
        {
            x.Configuration = redisSettings.Configuration;
            x.InstanceName = redisSettings.InstanceName;
        });
        
        return services;
    }
}