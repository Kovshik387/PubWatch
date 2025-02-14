namespace BackgroundDailyService.Domain.Entities;

public class Exchange
{
    public required string Date { get; init; }
    public IEnumerable<Currency> Currencies { get; init; } = [];
}