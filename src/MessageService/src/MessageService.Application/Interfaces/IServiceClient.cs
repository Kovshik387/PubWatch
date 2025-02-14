using MessageService.Domain;

namespace MessageService.Application.Interfaces;

public interface IServiceClient 
{
    public Task<string> GetCurrenciesAsync();
}