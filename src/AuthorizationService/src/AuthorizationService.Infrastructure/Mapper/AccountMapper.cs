using AccountServiceProto;
using AuthorizationService.Application.Response;
using AutoMapper;

namespace AuthorizationService.Infrastructure.Mapper;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<AddAccountResponse, AccountResponse>().ReverseMap();
    }
}