using BackgroundDailyService.Application.Interfaces;

namespace BackgroundDailyService.Presentation;

public class DailyBackgroundService(IDailyReceiverService dailyReceiver, ILogger<DailyBackgroundService> logger)
    : BackgroundService
{
    private const int TimeDifference = 3;
    private const int UpdateTime = 18;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(20 * 1000, stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            if ((DateTime.UtcNow.Hour + TimeDifference).Equals(UpdateTime))
            {
                try
                {
                    await dailyReceiver.SendNotificationAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
            }
            await Task.Delay(1000 * 60 * 35, stoppingToken);
        }
    }
}