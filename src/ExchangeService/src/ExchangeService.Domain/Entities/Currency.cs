namespace ExchangeService.Domain.Entities;

public sealed class Currency
{
    public int Id { get; set; }

    public string Idname { get; set; } = null!;

    public int Numcode { get; set; }

    public string Charcode { get; set; } = null!;

    public int Nominal { get; set; }

    public string Name { get; set; } = null!;

    public decimal Value { get; set; }

    public decimal Vunitrate { get; set; }

    public int Valcursid { get; set; }

    public Quotation Valcurs { get; set; } = null!;
}