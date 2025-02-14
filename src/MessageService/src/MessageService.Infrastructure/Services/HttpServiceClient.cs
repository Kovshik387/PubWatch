using MessageService.Application.Interfaces;
using MessageService.Domain;
using MessageService.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MessageService.Infrastructure.Services;

public class HttpServiceClient : IServiceClient
{
    private readonly HttpClient _httpClient;

    private readonly HttpEndPoint _endpoint;
    
    private readonly ILogger<HttpServiceClient> _logger;
    
    public HttpServiceClient(IOptions<HttpEndPoint> endpoint, ILogger<HttpServiceClient> logger)
    {
        _logger = logger;
        _endpoint = endpoint.Value;
        _httpClient = new HttpClient();
    }

    public async Task<string> GetCurrenciesAsync()
    {
        var response = await _httpClient.GetAsync(_endpoint.Url 
                                                  + DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1)
                                                      .ToString("dd.MM.yyyy"));
        
        _logger.LogInformation($"Get currencies response");
        
        return await response.Content.ReadAsStringAsync();
    }
}