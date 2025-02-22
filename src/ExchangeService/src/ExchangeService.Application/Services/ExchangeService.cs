using System.Globalization;
using AutoMapper;
using ExchangeService.Application.Data.Dto;
using ExchangeService.Application.Interfaces;
using ExchangeService.Application.Settings;
using ExchangeService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeService.Application.Services;

public class ExchangeService : IExchangeService
{
    private readonly ExternEndPointRoute _externEndPointRoute;
    
    private readonly IDbContext _dbContext;
    private readonly IHttpServiceClient _httpClient;
    private readonly ILogger<ExchangeService> _logger;
    private readonly IMapper _mapper;
    /// <summary>
    /// Общее количество валют
    /// </summary>
    private const int TypeSize = 42;

    public ExchangeService(IDbContext dbContext, IHttpServiceClient httpClient, ILogger<ExchangeService> logger, 
        IMapper mapper, IOptions<ExternEndPointRoute> externEndPointRoute)
    {
        _dbContext = dbContext; _httpClient = httpClient;
        _logger = logger; _mapper = mapper;
        _externEndPointRoute = externEndPointRoute.Value;
    }

    public async Task<QuotationDto?> GetRateByDateAsync(DateOnly? date = null)
    {
        var data = await _dbContext.Quotations.FirstOrDefaultAsync(x => x.Date.Equals(date));
        
        if (data is null)
        {
            _logger.LogInformation("{Url}",date is null ? _externEndPointRoute.UrlDaily : _externEndPointRoute.UrlDaily
                + "?date_req=" + date);
            var response = await _httpClient.FetchDataAsync<QuotationDto>(
                date is null ? _externEndPointRoute.UrlDaily : _externEndPointRoute.UrlDaily + "?date_req=" + 
                                                               date.Value.ToString("dd.mm.yyyy", CultureInfo.InvariantCulture)
            );
            
            if (response is null) return response;
            
            if (await _dbContext.Quotations.FirstOrDefaultAsync(x => x.Date.Equals(response.Date)) is not null) 
                return response;
            
            await _dbContext.Quotations.AddAsync(_mapper.Map<Quotation>(response)); await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Запись данных в бд");
            return response;
        }

        if (data.Volutes.Count < TypeSize)
        {
            var response = await _httpClient.FetchDataAsync<QuotationDto>(
                date is null ? _externEndPointRoute.UrlDaily : _externEndPointRoute.UrlDaily + "?date_req=" + 
                                                               date.Value.ToString("dd.mm.yyyy", CultureInfo.InvariantCulture)
            );
            
            if (response?.Volute is null) return response;
            
            var newVolutes = _mapper.Map<List<Currency>>(response.Volute).Except(data.Volutes);
            
            foreach(var item in newVolutes) data.Volutes.Add(item);
            
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Данные до записаны");
        }

        _logger.LogInformation("Данные из бд");
        return _mapper.Map<QuotationDto>(data);
    
    }

    public async Task<IList<RecordDto>> GetRateListByDateAsync(DateOnly date1, DateOnly date2, string nameVal)
    {
        var existingRecords = await _dbContext.Currencies
            .Where(v => v.Idname.Equals(nameVal) && v.Valcurs.Date >= date1 && v.Valcurs.Date <= date2)
            .Include(volute => volute.Valcurs)
            .ToListAsync();

        var existingDates = existingRecords.Select(v => v.Valcurs.Date).ToHashSet(); 

        var allDates = Enumerable.Range(0, date2.DayNumber - date1.DayNumber + 1)
                                 .Select(date1.AddDays)
                                 .ToList();

        var missingDates = allDates.Except(existingDates).ToList();

        var result = new List<RecordDto>();

        var voluteTemplate = await _dbContext.Currencies.
            FirstOrDefaultAsync(x => x.Idname.Equals(nameVal));

        if (missingDates.Count != 0)
        {
            var missingDateRanges = SplitDatesIntoRanges(missingDates);

            foreach (var dateRange in missingDateRanges)
            {
                var response = await _httpClient.FetchDataAsync<QuotationsDto>(
                    $"{_externEndPointRoute.UrlInterval}?date_req1={dateRange.Item1}&" +
                    $"date_req2={dateRange.Item2}&VAL_NM_RQ={nameVal}");
                _logger.LogInformation($"Запрос недостающих дат: {dateRange.Item1}\t|\t{dateRange.Item2}");

                if (response == null || response.Records.Count == 0)
                {
                    foreach (var date in missingDates.Where(d => d >= dateRange.Item1 && d <= dateRange.Item2))
                    {
                        await AddEmptyData(date);
                    }
                    continue;
                }

                foreach (var item in response.Records)
                {
                    var recordDate = DateOnly.Parse(item.Date);
                    item.Name = voluteTemplate!.Name;
                    result.Add(item);

                    if (existingDates.Contains(recordDate)) continue;
                    
                    var rateValue = await _dbContext.Quotations
                        .FirstOrDefaultAsync(rv => rv.Date == recordDate);
                    if (rateValue == null)
                    {
                        rateValue = new Quotation()
                        {
                            Date = recordDate,
                            Name = response.Name
                        };
                        _dbContext.Quotations.Add(rateValue);
                        await _dbContext.SaveChangesAsync();
                    }

                    var volute = new Currency
                    {
                        Idname = item.Id,
                        Nominal = item.Nominal,
                        Value = item.Value,
                        Vunitrate = item.VunitRate,
                        Valcurs = rateValue,
                        Charcode = voluteTemplate.Charcode,
                        Name = voluteTemplate.Name,
                        Numcode = voluteTemplate.Numcode,
                    };
                    await _dbContext.Currencies.AddAsync(volute);
                }
                var apiDates = response.Records.Select(r => DateOnly.Parse(r.Date)).ToHashSet();
                var missingApiDates = missingDates
                    .Where(d => d >= dateRange.Item1 && d <= dateRange.Item2 && !apiDates.Contains(d))
                    .ToList();

                foreach (var missingDate in missingApiDates)
                {
                    await AddEmptyData(missingDate);
                }
            }
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Данные из бд");
        }

        var combinedResult = existingRecords.Where(x => x.Valcurs.Name != "No data")
            .Select(x => new RecordDto()
        {
            Date = x.Valcurs.Date.ToString("dd.MM.yyyy"),
            Id = x.Idname,
            Name = x.Name,
            Nominal = x.Nominal,
            Value = x.Value,
            VunitRate = x.Vunitrate,
        }).ToList();

        combinedResult.AddRange(result);

        return [.. combinedResult.OrderBy(x => DateOnly.Parse(x.Date))];
    }
    
    private static List<(DateOnly, DateOnly)> SplitDatesIntoRanges(List<DateOnly> dates)
    {
        var ranges = new List<(DateOnly, DateOnly)>();
        
        if (dates.Count == 0) return ranges;

        dates.Sort();

        var startDate = dates.First();
        var endDate = startDate;

        foreach (var date in dates.Skip(1))
        {
            if (date.DayNumber == endDate.DayNumber + 1)
            {
                endDate = date;
            }
            else
            {
                ranges.Add((startDate, endDate));
                startDate = date;
                endDate = startDate;
            }
        }

        ranges.Add((startDate, endDate));
        return ranges;
    }
    
    private async Task AddEmptyData(DateOnly date)
    {
        var data = await _dbContext.Quotations.FirstOrDefaultAsync(x => x.Date == date)
                   ?? new Quotation { Date = date, Name = "No data" };

        if (data.Id == 0)
        {
            _dbContext.Quotations.Add(data);
            await _dbContext.SaveChangesAsync();
        }

        await _dbContext.SaveChangesAsync();
    }
}