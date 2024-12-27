namespace AuthorizationService.Application.Dto;

public record SignInDto(string Email, string Password, string Device);
