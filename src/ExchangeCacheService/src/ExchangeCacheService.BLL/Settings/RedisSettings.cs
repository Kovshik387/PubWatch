namespace ExchangeCacheService.BLL.Settings;

public class RedisSettings
{
    public string Configuration { get; set; } = default!;
    public string InstanceName { get; set; } = default!;
    public double CacheSmallData { get; set; } = default!;
    public double CacheLargeData { get; set;} = default!;
}