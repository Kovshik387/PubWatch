using AccountService.Application.Dto;
using AccountService.Application.Features.Commands;
using AccountService.Domain.Entities;
using AutoMapper;

namespace AccountService.Infrastructure.Mapper;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<AccountDto,User>().ReverseMap();
        CreateMap<FavoriteDto,Favorite>().ReverseMap();
    }
}