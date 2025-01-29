using AccountServiceProto;
using AuthorizationService.Application.Dto;
using AuthorizationService.Application.Interfaces;
using AuthorizationService.Infrastructure.Settings;
using AutoMapper;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthorizationService.Infrastructure.Services;

public class GrpcServiceClient : IServiceClient
{
    private readonly GrpcEndPointRoute _endpointRoute;
    
    private readonly IMapper _mapper;
    
    private readonly ILogger<GrpcServiceClient> _logger;
    
    public GrpcServiceClient(IOptions<GrpcEndPointRoute> endpointRoute, IMapper mapper, ILogger<GrpcServiceClient> logger)
    {
        _endpointRoute = endpointRoute.Value;
        _mapper = mapper; _logger = logger;
    }
    
    public async Task<TData> Send<TData>(AccountDto accountDto)
    {
        using var channel = GrpcChannel.ForAddress(_endpointRoute.Url);
        var client = new Account.AccountClient(channel);
        
        _logger.LogInformation($"Adding account: {accountDto.AccountId} to {_endpointRoute.Url}");
        
        var result = _mapper.Map<TData>(await client.AddAccountAsync(new AccountRequest()
        {
            Id = accountDto.AccountId.ToString(),
            Patronymic = accountDto.Patronymic,
            Name = accountDto.Name,
            Surname = accountDto.SurName,
            Email = accountDto.Email,
        }));

        return result;
    }
}