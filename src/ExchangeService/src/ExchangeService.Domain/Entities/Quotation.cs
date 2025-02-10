namespace ExchangeService.Domain.Entities;

public class Quotation
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Currency> Volutes { get; set; } = new List<Currency>();
}