using AccountService.Application.Features.Commands;
using AccountService.Application.Features.Queries;
using AccountServiceProto;
using AutoMapper;
using Grpc.Core;
using MediatR;

namespace AccountService.Api.Services;

public class ProfileService : Account.AccountBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    public ProfileService(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator; _mapper = mapper;
    }
    
    public override async Task<AccountsResponse> GetAccounts(AccountRequest request, ServerCallContext context)
    {
        var secretKey = context.RequestHeaders.FirstOrDefault(h => h.Key == "secret")?.Value;
    
        if (string.IsNullOrEmpty(secretKey) || secretKey != "")
        {
            throw new RpcException(new Status(
                StatusCode.PermissionDenied, 
                "Invalid or missing secret key"));
        }
        
        return _mapper.Map<AccountsResponse>(await _mediator.Send(new GetAccountsQuery()));
    }

    public override async Task<AccountResponse> AddAccount(AccountRequest request, ServerCallContext context)
    {
        var id = await _mediator.Send(new
            AddAccountCommand(Guid.Parse(request.Id), request.Name, request.Surname, request.Patronymic,
                request.Email));
        
        return new AccountResponse() { Id = id.ToString(), Success = true };
    }
}