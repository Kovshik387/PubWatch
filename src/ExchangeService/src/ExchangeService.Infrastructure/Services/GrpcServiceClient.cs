using System.Text;
using System.Xml.Serialization;
using ExchangeService.Application.Interfaces;
using ExchangeService.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace ExchangeService.Infrastructure.Services;

public class GrpcServiceClient : IServiceClient
{
    private readonly ILogger<GrpcServiceClient> _logger;

    public GrpcServiceClient(ILogger<GrpcServiceClient> logger)
    {
        _logger = logger;
    }

    public async Task<TData?> FetchDataAsync<TData>(string url)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var serializer = new XmlSerializer(typeof(TData));
        
        using var client = new HttpClient();
        _logger.LogInformation("!!!!!!!!!!!!!");
        var response = await client.GetAsync(url);
        
        _logger.LogInformation("{Url}", url);
        _logger.LogInformation($"{response.StatusCode}");
        if (!response.IsSuccessStatusCode) throw new ExternApiException($"Api CBR не доступно");

        await using var responseStream = await response.Content.ReadAsStreamAsync();

        using var reader = new StreamReader(responseStream, Encoding.GetEncoding("windows-1251"));
        var result = (TData)serializer.Deserialize(reader)!;
        return result;
    }
}