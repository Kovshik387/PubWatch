namespace AccountService.Application.Dto;

public class SignUpDto
{
    public Guid AccountId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SurName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;  
    public string Email { get; set; } = string.Empty;
}