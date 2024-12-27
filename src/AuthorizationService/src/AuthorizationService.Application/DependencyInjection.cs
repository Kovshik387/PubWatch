using Application.Dto;
using Application.Interfaces;
using Application.Services;
using Domain.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        
        services.AddFluentValidationAutoValidation(x => x.DisableDataAnnotationsValidation = true);
        
        services.AddValidatorsFromAssemblyContaining<SignInDto>();
        services.AddValidatorsFromAssemblyContaining<SignUpDto>();
        
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IJwtGenerator, JwtGenerator>();
        
        return services;
    }
}

