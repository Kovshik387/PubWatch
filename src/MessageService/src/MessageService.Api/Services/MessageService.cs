using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using MessageService.Application.Features;
using MessageServiceProto;

namespace MessageService.Api.Services;

public class MessageService(IMediator mediator, IMapper mapper) : Message.MessageBase
{
    private readonly IMapper _mapper = mapper;

    public override async Task<Empty> NotificationSubscribers(NotificationRequest request, ServerCallContext context)
    {
        await mediator.Send(new SendNotificationCommand(request.Email.ToList()));
        
        return new Empty();
    }
}