﻿using AccountService.Application.Dto;
using AccountService.Domain.Entities;
using AutoMapper;

namespace AccountService.Infrastructure.Mapper;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<UserDto,User>().ReverseMap();
        CreateMap<FavoriteDto,Favorite>().ReverseMap();
    }
}