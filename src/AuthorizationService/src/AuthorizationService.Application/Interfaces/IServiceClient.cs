using AuthorizationService.Application.Dto;
using AuthorizationService.Application.Response;

namespace AuthorizationService.Application.Interfaces;

public interface IServiceClient
{
    public Task<TData> Send<TData>(AccountDto accountDto);
}