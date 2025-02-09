namespace AccountService.Application.Dto;

public class AccountDto
{
    public string Name { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    public string? Patronymic { get; init; } = string.Empty;
    public IEnumerable<FavoriteDto> Favorites { get; init; } = [];
    public string Image { get; set; } = string.Empty;
}