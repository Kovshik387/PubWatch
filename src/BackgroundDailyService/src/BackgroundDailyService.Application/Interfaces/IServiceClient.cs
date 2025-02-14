using BackgroundDailyService.Domain.Entities;

namespace BackgroundDailyService.Application.Interfaces;

public interface IServiceClient
{
    public Task<TData?> GetDataAsync<TData>(CancellationToken cancellationToken = default);
    
    public Task SetDataAsync(IList<Domain.Entities.Account> models, CancellationToken cancellationToken = default);

    public bool CanSend(CommunicationType communicationType);
}

public enum CommunicationType
{
    Grpc,
    Http
}