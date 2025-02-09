using ExchangeService.Application.Data.Dto;

namespace ExchangeService.Application.Interfaces;

public interface IExchangeService
{
    /// <summary>
    /// Получение котировок за указанный день
    /// </summary>
    /// <param name="date">В случае, если аргумент равен null, будут получены данные за сегодняшнюю дату.</param>
    /// <returns>Перечисление <see cref="QuotationDto"/></returns>
    public Task<QuotationDto?> GetRateByDateAsync(DateOnly? date = null);
    /// <summary>
    /// Получение котировок за указанный интервал
    /// </summary>
    /// <param name="date1">Первая дата</param>
    /// <param name="date2">Вторая дата</param>
    /// <param name="nameVal">Имя валюты</param>
    /// <returns>Перечисление <see cref="CurrencyDto"/></returns>
    public Task<IList<RecordDto>> GetRateListByDateAsync(DateOnly date1, DateOnly date2, string nameVal);    
}