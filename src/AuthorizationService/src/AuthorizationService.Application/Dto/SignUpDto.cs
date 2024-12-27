namespace AuthorizationService.Application.Dto;

public record SignUpDto(string Email, string Password, string Name, string Surname, string Patronymic, string Device);