using System.Text;
using System.Xml.Serialization;
using ExchangeService.Application.Interfaces;
using ExchangeService.Infrastructure.Extensions;

namespace ExchangeService.Infrastructure.Services;

public class HttpServiceClient : IHttpServiceClient
{
    public async Task<TData?> FetchDataAsync<TData>(string url)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var serializer = new XmlSerializer(typeof(TData));
        
        using var client = new HttpClient();
        
        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode) throw new ExternApiException($"Api CBR не доступно");

        await using var responseStream = await response.Content.ReadAsStreamAsync();

        using var reader = new StreamReader(responseStream, Encoding.GetEncoding("windows-1251"));
        var result = (TData)serializer.Deserialize(reader)!;
        return result;
    }
}