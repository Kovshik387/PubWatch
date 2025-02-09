using System.Xml.Serialization;

namespace ExchangeService.Application.Data.Dto;

[XmlRoot("ValCurs")]
public class QuotationDto
{
    [XmlAttribute("Date")]
    public string Date { get; set; } = string.Empty;
    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;
    [XmlElement(ElementName = "Valute")]
    public List<CurrencyDto> Volute { get; set; } = [];
}