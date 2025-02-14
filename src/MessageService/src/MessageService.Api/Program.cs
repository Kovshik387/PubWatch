using CoreConfiguration;
using MessageService.Application;
using MessageService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.UseAppLogger();
builder.UseAppAuth();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCoreConfiguration(builder.Configuration);
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddControllers();

var app = builder.Build();

app.UseCoreConfiguration();
app.UseHttpsRedirection();
app.MapControllers();

app.MapGrpcService<MessageService.Api.Services.MessageService>();

app.Run();