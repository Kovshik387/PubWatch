using CoreConfiguration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StorageService.Api.Services;
using StorageService.Application;
using StorageService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.UseAppLogger();
builder.UseAppAuth();
builder.Services.AddGrpc().AddJsonTranscoding();;
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddCoreConfiguration(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseCoreConfiguration();
app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => "Hello World!");
app.UseHealthChecks("/health");
app.MapHealthChecks("/health");

app.MapGrpcService<StorageFilesService>();

app.Run();