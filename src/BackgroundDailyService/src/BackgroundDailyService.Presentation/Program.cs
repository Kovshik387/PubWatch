using BackgroundDailyService.Application;
using BackgroundDailyService.Infrastructure;
using BackgroundDailyService.Presentation;
using MessagePublisher;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMessagePublisher(builder.Configuration);
builder.Services.AddHostedService<DailyBackgroundService>();
builder.Services.AddHostedService<ReceiverBackgroundService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.Run();