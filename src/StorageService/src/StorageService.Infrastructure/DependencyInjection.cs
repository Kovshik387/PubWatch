using Amazon.S3;
using StorageService.Application.Settings;
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
                ServiceURL = settings.EndPoint,
                ForcePathStyle = true,
                Timeout = TimeSpan.FromSeconds(10),
                UseHttp = true,
            }));
        
        return services;
    }
}