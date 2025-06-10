namespace ExchangeService.Application.Interfaces;

public interface IServiceClient
{
    public Task<TData?> FetchDataAsync<TData>(string url);
}