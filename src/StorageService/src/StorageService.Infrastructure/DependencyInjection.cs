using System.Net;
using Amazon;
using Amazon.Internal;
using Amazon.S3;
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

        return services.AddS3(configuration);
    }

    private static IServiceCollection AddS3(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(StorageSettings)).Get<StorageSettings>();

        if (settings == null) throw new ArgumentNullException(nameof(settings));
        
        services.AddSingleton<IAmazonS3>(x => new AmazonS3Client(
            settings.AccessKey, settings.SecretKey, new AmazonS3Config()
            {
                RegionEndpoint = RegionEndpoint.USEast1,
                ServiceURL = settings.EndPoint,
                ForcePathStyle = true,
                AuthenticationRegion = RegionEndpoint.USEast1.SystemName,
                //TODO ssl
                UseHttp = true,
            }));
        
        return services;
    }
}