using AccountServiceProto;
using AutoMapper;
using BackgroundDailyService.Application.Interfaces;
using BackgroundDailyService.Infrastructure.Settings;
using Grpc.Core;
using Grpc.Net.Client;
using MessageServiceProto;
using Microsoft.Extensions.Options;

namespace BackgroundDailyService.Infrastructure.Services;

public class GrpcServiceClient : IServiceClient
{
    private readonly IMapper _mapper;
    
    private readonly EndPoints _endpoints;
    private readonly SecretSettings _secretSettings;
    
    public GrpcServiceClient(IOptions<EndPoints> endpoints, IMapper mapper, IOptions<SecretSettings> secretSettings)
    {
        _mapper = mapper;
        _secretSettings = secretSettings.Value;
        _endpoints = endpoints.Value;
    }

    public async Task<TData?> GetDataAsync<TData>(CancellationToken cancellationToken = default)
    {
        using var channel = GrpcChannel.ForAddress(_endpoints.GrpcUrlGet);
        var client = new Account.AccountClient(channel);

        var result = await client.GetAccountsAsync(new AccountRequest(),
            new Metadata { { "secret", _secretSettings.Secret } },
            cancellationToken: cancellationToken);
        
        return  _mapper.Map<TData>(result.Accounts);
    }

    public async Task SetDataAsync(IList<Domain.Entities.Account> models, CancellationToken cancellationToken = default)
    {
        using var channel = GrpcChannel.ForAddress(_endpoints.GrpcUrlSend);
        var client = new Message.MessageClient(channel);

        var a  = await client.NotificationSubscribersAsync(new NotificationRequest()
        {
           Email = { models.Select(x => x.Email) }
        }, new CallOptions().WithWaitForReady(false));
    }

    public bool CanSend(CommunicationType communicationType) => communicationType.Equals(CommunicationType.Grpc);
}