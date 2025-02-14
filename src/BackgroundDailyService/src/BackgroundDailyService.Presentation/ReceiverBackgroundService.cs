using BackgroundDailyService.Application.Interfaces;
using BackgroundDailyService.Application.Settings;
using BackgroundDailyService.Domain.Entities;
using MessagePublisher.Interfaces;
using Microsoft.Extensions.Options;

namespace BackgroundDailyService.Presentation;

public class ReceiverBackgroundService : BackgroundService
{
    private readonly QueueSettings _queueSettings;
    private readonly IMessagePublisher _messagePublisher;
    private readonly IDailyReceiverService _dailyReceiverService;
    
    public ReceiverBackgroundService(IOptions<QueueSettings> queueSettings, IMessagePublisher messagePublisher,
        IDailyReceiverService dailyReceiverService)
    {
        _messagePublisher = messagePublisher;
        _dailyReceiverService = dailyReceiverService;
        _queueSettings = queueSettings.Value;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _messagePublisher.Subscribe<List<Account>>(_queueSettings.PublishQueueName,
                _dailyReceiverService.SendEmailAsync);
        }
    }
}