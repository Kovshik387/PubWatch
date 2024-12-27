namespace AuthorizationService.Application.Dto;

public record AuthDto(Guid Id, string AccessToken, string RefreshToken);