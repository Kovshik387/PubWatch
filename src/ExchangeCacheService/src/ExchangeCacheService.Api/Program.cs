using CoreConfiguration;
using ExchangeCacheService.BLL;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.UseAppLogger();
builder.UseAppAuth();

builder.Services.AddBllLayer(builder.Configuration);
builder.Services.AddCoreConfiguration(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseCoreConfiguration();
app.UseHttpsRedirection();
app.MapControllers();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHealthChecks("/health");
app.MapHealthChecks("/health");

app.Run();