using AccountService.Api.Settings;
using AccountService.Application.Dto;
using AccountService.Application.Features.Commands;
using AccountService.Application.Features.Queries;
using AccountServiceProto;
using AutoMapper;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;

namespace AccountService.Api.Services;

public class ProfileService : Account.AccountBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly SecretSettings _secretSettings;
    public ProfileService(IMediator mediator, IMapper mapper, IOptions<SecretSettings> secretSettings)
    {
        _mediator = mediator; _mapper = mapper;
        _secretSettings = secretSettings.Value;
    }
    
    public override async Task<AccountsResponse> GetAccounts(AccountRequest request, ServerCallContext context)
    {
        var secretKey = context.RequestHeaders.FirstOrDefault(h => h.Key == "secret")?.Value;
    
        if (string.IsNullOrEmpty(secretKey) || secretKey != _secretSettings.Secret)
        {
            throw new RpcException(new Status(
                StatusCode.PermissionDenied, 
                "Invalid or missing secret key"));
        }

        return _mapper.Map<AccountsResponse>(new SubscribersDto()
        {
            Accounts = await _mediator.Send(new GetAccountsQuery())
        });
    }

    public override async Task<AccountResponse> AddAccount(AccountRequest request, ServerCallContext context)
    {
        var id = await _mediator.Send(new
            AddAccountCommand(Guid.Parse(request.Id), request.Name, request.Surname, request.Patronymic,
                request.Email));
        
        return new AccountResponse() { Id = id.ToString(), Success = true };
    }
}