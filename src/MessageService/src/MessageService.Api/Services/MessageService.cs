using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using MessageService.Application.Features;
using MessageServiceProto;

namespace MessageService.Api.Services;

public class MessageService : Message.MessageBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    public MessageService(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override async Task<Empty> NotificationSubscribers(NotificationRequest request, ServerCallContext context)
    {
        await _mediator.Send(new SendNotificationCommand(request.Email.ToList()));
        
        return new Empty();
    }
}