using AccountService.Application.Dto;
using AccountService.Domain.Entities;
using AccountServiceProto;
using AutoMapper;

namespace AccountService.Infrastructure.Mapper;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<AccountDto,User>().ReverseMap();
        CreateMap<FavoriteDto,Favorite>().ReverseMap();
        CreateMap<SubscribersDto,AccountsResponse>().ReverseMap();
        CreateMap<AccountEmailDto,User>().ReverseMap();
        CreateMap<AccountEmailDto,AccountSubscribe>().ReverseMap();
    }
}