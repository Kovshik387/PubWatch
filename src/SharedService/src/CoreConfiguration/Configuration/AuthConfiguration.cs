using System.Net;
using CoreConfiguration.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CoreConfiguration.Configuration;

public static class AuthConfiguration
{
    public static IServiceCollection AddAppAuth(this IServiceCollection services, JwtSettings? settings)
    {
        if (settings is null) return services;
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = settings.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = settings.SymmetricSecurityKeyAccess,
                    ValidateAudience = true,
                    ValidAudience = settings.Audience,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
                options.UseSecurityTokenValidators = true;
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return context.Response.WriteAsync("{\"error\":\"Unauthorized access\"}");
                    }
                };
            });

        return services;
    }

    public static IApplicationBuilder UseAppAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();

        app.UseAuthorization();

        return app;
    }
}