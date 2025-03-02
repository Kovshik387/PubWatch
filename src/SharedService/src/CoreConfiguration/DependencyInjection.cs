using CoreConfiguration.Configuration;
using CoreConfiguration.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreConfiguration;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppCors();
        services.AddAppSwagger(configuration);
        
        return services;
    }
    
    public static void UseCoreConfiguration(this WebApplication app)
    {
        app.UseAppCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAppSwagger();
    }

    public static void UseAppAuth(this WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        builder.Services.AddAppAuth(jwtSettings);
    }
    
    public static void UseAppLogger(this WebApplicationBuilder builder)
    {
        var loggerSettings = builder.Configuration.GetSection(nameof(LoggerSettings)).Get<LoggerSettings>();
        builder.AddAppLogger(loggerSettings!);
    }
}