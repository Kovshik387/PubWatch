using System.Globalization;
using BackgroundDailyService.Application.Interfaces;
using BackgroundDailyService.Application.Settings;
using BackgroundDailyService.Domain.Entities;
using MessagePublisher.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BackgroundDailyService.Application.Services;

public class DailyReceiverService : IDailyReceiverService
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly IEnumerable<IServiceClient> _communicationServices;
    private readonly ILogger<DailyReceiverService> _logger;

    private readonly QueueSettings _queueSettings;
    
    private const int ChunkSize = 5;
    
    public DailyReceiverService(IMessagePublisher messagePublisher, 
        ILogger<DailyReceiverService> logger, IOptions<QueueSettings> queueSettings,
        IEnumerable<IServiceClient> communicationServices)
    {
        _messagePublisher = messagePublisher; 
        _logger = logger;
        _communicationServices = communicationServices;
        _queueSettings = queueSettings.Value;
    }
    
    public async Task SendNotificationAsync()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);

        var httpService = GetCommunicationService(CommunicationType.Http);
        
        var responseData = await httpService.GetDataAsync<Exchange>();
        
        if (responseData is null) return;
        
        if (!DateOnly.TryParseExact(
                responseData.Date.Trim(),
                "dd.MM.yyyy",            
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var responseDate))
        {
            _logger.LogError($"Invalid date format: {responseData.Date}");
            return;
        }
        
        if (responseDate < date)
        {
            _logger.LogInformation($"No Currencies on {date}");
            return;
        }
        
        var grpcService = GetCommunicationService(CommunicationType.Grpc);
        
        var accounts = await grpcService.GetDataAsync<IReadOnlyCollection<Account>>();

        if (accounts is null || accounts.Count == 0) return;
        
        foreach (var item in accounts.Chunk(ChunkSize))
        {
            await _messagePublisher.PushAsync(_queueSettings.PublishQueueName, item);
        }
    }

    public async Task SendEmailAsync(IList<Account> accounts)
    {
        var grpcService = GetCommunicationService(CommunicationType.Grpc);
        await grpcService.SetDataAsync(accounts);
    }

    private IServiceClient GetCommunicationService(CommunicationType communicationType)
    {
        foreach (var item in _communicationServices)
        {
            if (item.CanSend(communicationType))
            {
                return item;
            }
        }

        throw new ArgumentOutOfRangeException();
    }
}