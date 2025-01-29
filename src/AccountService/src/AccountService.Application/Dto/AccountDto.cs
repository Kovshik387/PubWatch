namespace AccountService.Application.Dto;

public record AccountDto(string Name, string Surname,string? Patronymic, IEnumerable<FavoriteDto> Favorites);