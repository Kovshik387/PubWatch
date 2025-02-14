using AutoMapper;
using ExchangeService.Application.Data.Dto;
using ExchangeService.Domain.Entities;

namespace ExchangeService.Infrastructure.Mapper;

public class CurrencyMapper : Profile
{
    public CurrencyMapper()
    {
        CreateMap<CurrencyDto, Currency>()
            .ForMember(dest => dest.Idname, opt => opt.MapFrom(src => src.ID))
            .ForMember(dest => dest.Numcode, opt => opt.MapFrom(src => src.NumCode))
            .ForMember(dest => dest.Charcode, opt => opt.MapFrom(src => src.CharCode))
            .ForMember(dest => dest.Nominal, opt => opt.MapFrom(src => src.Nominal))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.Vunitrate, opt => opt.MapFrom(src => src.VunitRate))
            .ForMember(dest => dest.Valcursid, opt => opt.Ignore())
            .ForMember(dest => dest.Valcurs, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Idname))
            .ForMember(dest => dest.NumCode, opt => opt.MapFrom(src => src.Numcode))
            .ForMember(dest => dest.CharCode, opt => opt.MapFrom(src => src.Charcode))
            .ForMember(dest => dest.Nominal, opt => opt.MapFrom(src => src.Nominal))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ValueString, opt => opt.Ignore())
            .ForMember(dest => dest.VunitRateString, opt => opt.Ignore());
            
        CreateMap<Quotation, QuotationDto>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("dd.MM.yyyy")))
            .ForMember(dest => dest.Volute, opt => opt.MapFrom(src => src.Volutes))
            .ReverseMap()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.ParseExact(src.Date, "dd.MM.yyyy")))
            .ForMember(dest => dest.Volutes, opt => opt.MapFrom(src => src.Volute));
    }
}