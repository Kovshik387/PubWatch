using AutoMapper;
using MessageService.Application.Features;
using MessageService.Domain;
using MessageServiceProto;

namespace MessageService.Infrastructure.Mapper;

public class MessageMapper : Profile
{
    public MessageMapper()
    {
        CreateMap<NotificationRequest,SendNotificationCommand>().ReverseMap();
    }
}