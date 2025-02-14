using AccountService.Api.Middleware;
using AccountService.Api.Services;
using AccountService.Api.Settings;
using AccountService.Application;
using AccountService.Infrastructure;
using AccountService.Infrastructure.Data;
using CoreConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SecretSettings>(builder.Configuration.GetSection(nameof(SecretSettings)));

builder.UseAppLogger();
builder.UseAppAuth();
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddCoreConfiguration(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseCoreConfiguration();
// app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => "Hello World!");
app.UseHealthChecks("/health");
app.MapHealthChecks("/health");

app.UseMiddleware<ExceptionMiddleware>();

DbInitializer.Execute(app.Services);

app.MapGrpcService<ProfileService>();

app.Run();