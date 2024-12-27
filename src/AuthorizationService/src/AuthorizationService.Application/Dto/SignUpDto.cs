namespace AuthorizationService.Application.Dto;

public class SignUpDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Patronymic { get; set; }
    public string? Device { get; set; }
}