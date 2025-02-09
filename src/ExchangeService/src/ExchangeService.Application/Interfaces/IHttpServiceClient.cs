namespace ExchangeService.Application.Interfaces;

public interface IHttpServiceClient
{
    public Task<TData?> FetchDataAsync<TData>(string url);
}