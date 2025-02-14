using BackgroundDailyService.Domain.Entities;

namespace BackgroundDailyService.Application.Interfaces;

public interface IDailyReceiverService
{
    public Task SendNotificationAsync();
    public Task SendEmailAsync(IList<Account> accounts);
}