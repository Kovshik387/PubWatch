using AutoMapper;
using ExchangeService.Application.Data.Dto;
using ExchangeService.Domain.Entities;

namespace ExchangeService.Infrastructure.Mapper;

public class CurrencyMapper : Profile
{
    public CurrencyMapper()
    {
        CreateMap<Currency, CurrencyDto>().ReverseMap();
        CreateMap<QuotationDto, Quotation>().ReverseMap();
    }
}