using CoreConfiguration;
using ExchangeService.Application;
using ExchangeService.Infrastructure;
using ExchangeService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.UseAppLogger();
builder.UseAppAuth();
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddCoreConfiguration(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseCoreConfiguration();
app.UseHttpsRedirection();
app.MapControllers();

app.UseHealthChecks("/health");
app.MapHealthChecks("/health");

DbInitializer.Execute(app.Services);

app.MapGrpcService<ExchangeService.Api.Services.ExchangeService>();

app.Run();