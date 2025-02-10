using ExchangeCacheService.BLL.Interfaces;
using ExchangeCacheService.BLL.Settings;
using ExchangeServiceProto;
using Grpc.Net.Client;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ExchangeCacheService.BLL.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly RedisSettings _redisSettings;
    private readonly ILogger<CacheService> _logger;
    private readonly GrpcEndPointRoute _endpointRoute;
    private readonly JsonSerializerSettings _jsonSerializerSettings;
    
    private const string DateFormatString = "dd.MM.yyyy";
    
    public CacheService(IDistributedCache cache, IOptions<GrpcEndPointRoute> endpointRoute,
        IOptions<RedisSettings> redisSettings, ILogger<CacheService> logger)
    {
        _cache = cache;
        _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = DateFormatString,
        };
        _logger = logger;
        _redisSettings = redisSettings.Value;
        _endpointRoute = endpointRoute.Value;
    }
    
    public async Task<DailyVoluteResponse?> GetDataByDateAsync(string date)
    {
        return await GetDataVolute(date,
            async () => await GetDailyVoluteFromSource(date),
            TimeSpan.FromMinutes(_redisSettings.CacheSmallData));
    }

    public async Task<DynamicValueResponse?> GetDataByDatesAsync(string date1, string date2, string name)
    {
        var searchString = $"{date1} {date2} {name}";
        return await GetDataVolute(searchString,
            async () => await GetDynamicValueFromSource(date1, date2, name),
            TimeSpan.FromMinutes(_redisSettings.CacheLargeData));
    }
    
    private async Task<TData?> GetDataVolute<TData>(string cacheKey, Func<Task<TData>> getDataFromSource,
        TimeSpan cacheDuration)
    {
        TData? result = default;
        var requestString = await _cache.GetStringAsync(cacheKey);

        if (requestString is not null)
        {
            result = JsonConvert.DeserializeObject<TData>(requestString, _jsonSerializerSettings);
        }

        if (result is null)
        {
            result = await getDataFromSource();

            var serializedResult = JsonConvert.SerializeObject(result, _jsonSerializerSettings);
            await _cache.SetStringAsync(cacheKey, serializedResult, new DistributedCacheEntryOptions
            {
                SlidingExpiration = cacheDuration
            });

            _logger.LogInformation("Data from database");
        }
        else
        {
            _logger.LogInformation("Data from cache");
        }

        return result;
    }

    private async Task<DailyVoluteResponse> GetDailyVoluteFromSource(string date)
    {
        using var channel = GrpcChannel.ForAddress(_endpointRoute.Url);
        var client = new Volute.VoluteClient(channel);
        return await client.GetCurrentValueAsync(new DailyVoluteRequest { Date = date });
    }

    private async Task<DynamicValueResponse> GetDynamicValueFromSource(string date1, string date2, string name)
    {
        using var channel = GrpcChannel.ForAddress(_endpointRoute.Url);
        var client = new Volute.VoluteClient(channel);
        return await client.GetDynamicValueAsync(new DynamicValueRequest { Date1 = date1, Date2 = date2, Name = name });
    }
}