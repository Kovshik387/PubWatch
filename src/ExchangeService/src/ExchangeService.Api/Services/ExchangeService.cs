using System.Globalization;
using AutoMapper;
using ExchangeService.Application.Interfaces;
using Grpc.Core;

namespace ExchangeService.Api.Services;
using ExchangeServiceProto;

public class ExchangeService : Volute.VoluteBase
{
    private const string Locale = "ru-RU";
    private const string DateStringFormat = "dd.MM.yyyy";
    
    private readonly IExchangeService _exchangeService;
    private readonly IMapper _mapper; 
    private readonly ILogger<ExchangeService> _logger;
    
    public ExchangeService(IExchangeService exchangeService, IMapper mapper, ILogger<ExchangeService> logger)
    {
        _exchangeService = exchangeService;
        _mapper = mapper;
        _logger = logger;
    }

    public override async Task<DailyVoluteResponse> GetCurrentValue(DailyVoluteRequest request, ServerCallContext context)
    {
        if (!DateOnly.TryParseExact(request.Date, DateStringFormat, CultureInfo.GetCultureInfo(Locale),
                DateTimeStyles.None, out var parsedDate)) 
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid date format"));
        
        _logger.LogInformation($"Date: {request.Date}|\tParsed Date: {parsedDate}");
        
        return _mapper.Map<DailyVoluteResponse>(await _exchangeService.GetRateByDateAsync(parsedDate));
    }

    public override async Task<DynamicValueResponse> GetDynamicValue(DynamicValueRequest request, ServerCallContext context)
    {
        if (!DateOnly.TryParseExact(request.Date1, DateStringFormat, CultureInfo.GetCultureInfo(Locale),
            DateTimeStyles.None, out var parsedDate1)) 
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid date format"));

        if (!DateOnly.TryParseExact(request.Date2, DateStringFormat, CultureInfo.GetCultureInfo(Locale),
            DateTimeStyles.None, out var parsedDate2))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid date format"));
        
        var records = await _exchangeService.GetRateListByDateAsync(parsedDate1, parsedDate2, request.Name);
        
        var response = new DynamicValueResponse();
        response.Record.AddRange(_mapper.Map<List<RecordResponse>>(records));
        
        if (response.Record.Count != 0) throw new RpcException(new Status(StatusCode.NotFound, "No records found"));
        
        return response;
    }
}