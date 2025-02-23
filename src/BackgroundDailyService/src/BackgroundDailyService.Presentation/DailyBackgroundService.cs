using BackgroundDailyService.Application.Interfaces;

namespace BackgroundDailyService.Presentation;

public class DailyBackgroundService : BackgroundService
{
    private readonly IDailyReceiverService _dailyReceiver;
    private readonly ILogger<DailyBackgroundService> _logger;

    private const int TimeDifference = 3;
    private const int UpdateTime = 18;
    
    public DailyBackgroundService(IDailyReceiverService dailyReceiver, ILogger<DailyBackgroundService> logger)
    {
        _dailyReceiver = dailyReceiver;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(20 * 1000, stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            // if ((DateTime.UtcNow.Hour + TimeDifference).Equals(UpdateTime))
            // {
                try
                {
                    await _dailyReceiver.SendNotificationAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            // }
            await Task.Delay(1000 * 60 * 1, stoppingToken);
        }
    }
}