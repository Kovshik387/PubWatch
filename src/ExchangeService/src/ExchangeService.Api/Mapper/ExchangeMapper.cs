using AutoMapper;
using ExchangeService.Application.Data.Dto;
using ExchangeServiceProto;

namespace ExchangeService.Api.Mapper;

public class ExchangeMapper : Profile
{
    public ExchangeMapper()
    {
        CreateMap<DailyVoluteResponse,QuotationDto>().ReverseMap();
        CreateMap<RecordDto, DynamicValueResponse>().ReverseMap();
    }
}