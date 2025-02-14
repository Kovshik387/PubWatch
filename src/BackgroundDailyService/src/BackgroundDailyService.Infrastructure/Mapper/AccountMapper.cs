using AccountServiceProto;
using AutoMapper;

namespace BackgroundDailyService.Infrastructure.Mapper;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<AccountSubscribe, Domain.Entities.Account>().ReverseMap();
    }
}