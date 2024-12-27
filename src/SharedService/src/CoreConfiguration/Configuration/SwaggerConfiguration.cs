using System.Reflection;
using CoreConfiguration.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CoreConfiguration.Configuration;

public static class SwaggerConfiguration
{
    /// <summary>
    /// Add OpenAPI to API
    /// </summary>
    /// <param name="services">Services collection</param>
    /// <param name="configuration">Configuration</param>
    internal static IServiceCollection AddAppSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var swaggerSettings = configuration.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
        
        if (swaggerSettings is null) return services;
        
        if (!swaggerSettings.Enabled)
            return services;
        
        services
            .AddOptions<SwaggerGenOptions>();
        
        services.AddSwaggerGen(options =>
        {
            options.SupportNonNullableReferenceTypes();
            options.UseInlineDefinitionsForEnums();
            options.DescribeAllParametersInCamelCase();
            options.CustomSchemaIds(x => x.FullName);

            var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Insert JWT with Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });

            options.EnableAnnotations();
        
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = $"{swaggerSettings.AppTitle} API",
            });
        
            options.UseOneOfForPolymorphism();
            options.EnableAnnotations(true, true);
        
            options.UseAllOfForInheritance();
            options.UseOneOfForPolymorphism();
        
            options.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

        services.AddSwaggerGenNewtonsoftSupport();

        return services;
    }


    /// <summary>
    /// Start OpenAPI UI
    /// </summary>
    /// <param name="app">Web application</param>
    internal static void UseAppSwagger(this WebApplication app)
    {
        var swaggerSettings = app.Services.GetService<SwaggerSettings>();

        if (!swaggerSettings?.Enabled ?? false)
            return;
        
        app.UseSwagger();

        app.UseSwaggerUI(
            options =>
            {
                options.DocExpansion(DocExpansion.List);
                options.DefaultModelsExpandDepth(-1);
                options.OAuthAppName(swaggerSettings?.AppTitle);
            }
        );
    }
}