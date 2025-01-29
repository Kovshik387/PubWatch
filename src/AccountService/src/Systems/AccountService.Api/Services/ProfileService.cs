using AccountService.Application.Features.Commands;
using AccountServiceProto;
using Grpc.Core;
using MediatR;

namespace AccountService.Api.Services;

public class ProfileService : Account.AccountBase
{
    private readonly ILogger<ProfileService> _logger;
    private readonly IMediator _mediator;
    
    public ProfileService(ILogger<ProfileService> logger, IMediator mediator)
    {
        _logger = logger; _mediator = mediator;
    }

    public override async Task<AccountResponse> AddAccount(AccountRequest request, ServerCallContext context)
    {
        var id = await _mediator.Send(new
            AddAccountCommand(Guid.Parse(request.Id), request.Name, request.Surname, request.Patronymic,
                request.Email));
        
        return new AccountResponse() { Id = id.ToString(), Success = true };
    }
}