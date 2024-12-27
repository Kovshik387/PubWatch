namespace AccountService.Application.Dto;

public record UserDto(string Name, string Email, IEnumerable<FavoriteDto> Favorites);