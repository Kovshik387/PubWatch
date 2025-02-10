using ExchangeServiceProto;

namespace ExchangeCacheService.BLL.Interfaces;

public interface ICacheService
{
    public Task<DailyVoluteResponse?> GetDataByDateAsync(string date);
    
    public Task<DynamicValueResponse?> GetDataByDatesAsync(string date1, string date2, string name);
}