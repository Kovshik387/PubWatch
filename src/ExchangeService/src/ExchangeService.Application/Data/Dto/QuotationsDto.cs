using System.Xml.Serialization;

namespace ExchangeService.Application.Data.Dto;

public class QuotationsDto
{
    [XmlAttribute("ID")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("DateRange1")]
    public string DateRange1 { get; set; } = string.Empty;

    [XmlAttribute("DateRange2")]
    public string DateRange2 { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;
    [XmlElement("Record")]
    public List<RecordDto> Records { get; set; } = [];
}