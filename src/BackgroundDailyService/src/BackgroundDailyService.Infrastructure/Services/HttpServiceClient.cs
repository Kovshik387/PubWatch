using BackgroundDailyService.Application.Interfaces;
using BackgroundDailyService.Domain.Entities;
using BackgroundDailyService.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BackgroundDailyService.Infrastructure.Services;

public class HttpServiceClient : IServiceClient
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly EndPoints _endPoints;

    public HttpServiceClient(IOptions<EndPoints> httpEndPoint)
    {
        _endPoints = httpEndPoint.Value;
    }
    
    public async Task<TData?> GetDataAsync<TData>(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(_endPoints.HttpUrl 
                                                  + DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1)
                                                      .ToString("dd.MM.yyyy"),
            cancellationToken);
        
        return JsonConvert.DeserializeObject<TData>(await response.Content.ReadAsStringAsync(cancellationToken));
    }

    public async Task SetDataAsync(IList<Account> models, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public bool CanSend(CommunicationType communicationType) => communicationType.Equals(CommunicationType.Http);
}