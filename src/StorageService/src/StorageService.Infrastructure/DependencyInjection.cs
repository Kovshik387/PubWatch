using System.Net;
using Minio;
using StorageService.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StorageService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StorageSettings>(configuration.GetSection(nameof(StorageSettings)));
    
        var settings = configuration.GetSection(nameof(StorageSettings)).Get<StorageSettings>();
        
        if (settings == null) throw new ArgumentNullException(nameof(settings));
        
        services.AddMinio(x => x
            .WithEndpoint(settings.EndPoint)
            .WithProxy(new WebProxy(settings.Proxy,settings.ProxyPort))
            .WithCredentials(settings.AccessKey,settings.SecretKey)
            .WithSSL(settings.Ssl)
        );
        return services;
    }
}