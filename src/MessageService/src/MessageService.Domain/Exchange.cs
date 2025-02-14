namespace MessageService.Domain;

public class Exchange
{
    public required string Date { get; init; }
    public required IReadOnlyCollection<Currency> Volute { get; init; }
}