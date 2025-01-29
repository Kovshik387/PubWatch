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
        services.AddAppSwagger(configuration);
        services.AddAppCors();
        
        return services;
    }
    
    public static void UseCoreConfiguration(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAppSwagger();
        app.UseAppCors();
    }

    public static void UseAppAuth(this WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection(typeof(JwtSettings).ToString()).Get<JwtSettings>();
        builder.Services.AddAppAuth(jwtSettings);
    }
    
    public static void UseAppLogger(this WebApplicationBuilder builder)
    {
        var loggerSettings = builder.Configuration.GetSection(nameof(LoggerSettings)).Get<LoggerSettings>();
        builder.AddAppLogger(loggerSettings!);
    }
}