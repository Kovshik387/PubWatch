using ConfigurationShare.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigurationShare.Configuration;


public static class CorsConfiguration
{
    /// <summary>
    /// Add CORS
    /// </summary>
    /// <param name="services">Services collection</param>
    public static IServiceCollection AddAppCors(this IServiceCollection services)
    {
        services.AddCors();

        return services;
    }

    /// <summary>
    /// Use service
    /// </summary>
    /// <param name="app">Application</param>
    public static void UseAppCors(this WebApplication app)
    {
        var corsSettings = app.Services.GetService<CorsSettings>();

        var origins = corsSettings!.AllowedOrigins.Split(',', ';').Select(x => x.Trim())
            .Where(x => !string.IsNullOrEmpty(x)).ToArray();

        app.UseCors(pol =>
        {
            pol.AllowAnyHeader();
            pol.AllowAnyMethod();
            pol.AllowCredentials();
            if (origins.Length > 0)
                pol.WithOrigins(origins);
            else
                pol.SetIsOriginAllowed(origin => true);

            pol.WithExposedHeaders("Content-Disposition");
        });
    }
}