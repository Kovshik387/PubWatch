using System.Globalization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ExchangeService.Application.Data.Dto;

public sealed class RecordDto
{
    [XmlAttribute("Date")]
    public string Date { get; set; } = string.Empty;

    [XmlAttribute("Id")]
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Nominal { get; set; }
    [XmlIgnore]
    public decimal Value { get; set; } = default;

    [XmlElement("Value")]
    [JsonIgnore]
    public string ValueString
    {
        get => Value.ToString(CultureInfo.InvariantCulture);
        set => Value = decimal.Parse(value, CultureInfo.GetCultureInfo("ru-RU"));
    }
    [XmlIgnore]
    public decimal VunitRate { get; set; } = default;
    [JsonIgnore]
    [XmlElement("VunitRate")]
    public string VunitRateString
    {
        get => VunitRate.ToString(CultureInfo.InvariantCulture);
        set => VunitRate = decimal.Parse(value, CultureInfo.GetCultureInfo("ru-RU"));
    }
}