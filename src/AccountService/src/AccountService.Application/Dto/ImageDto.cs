namespace AccountService.Application.Dto;

public class ImageDto
{
    public byte[] Image { get; set; } = [];
    public string ImageFormat {get; set;} = "image/png";
    public string AccountId { get; set; } = string.Empty;
}