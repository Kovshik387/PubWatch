namespace MessageService.Application.Settings;

public class EmailSettings
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Provider { get; set; } = null!;
    public int Port { get; set; } = default!;
}