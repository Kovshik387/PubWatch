using AuthorizationService.Api.Middleware;
using AuthorizationService.Application;
using CoreConfiguration;
using AuthorizationService.Infrastructure;
using AuthorizationService.Infrastructure.Data;
using MessagePublisher;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMessagePublisher(builder.Configuration);
builder.UseAppLogger();
builder.Services.AddCoreConfiguration(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseCoreConfiguration();

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => "Hello World!");
app.UseHealthChecks("/health");
app.MapHealthChecks("/health");

app.UseMiddleware<ExceptionMiddleware>();

DbInitializer.Execute(app.Services);

app.Run();