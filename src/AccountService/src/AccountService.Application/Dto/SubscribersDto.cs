namespace AccountService.Application.Dto;

public class SubscribersDto
{
    public required List<AccountEmailDto> Accounts { get; set; } = [];
}

public record AccountEmailDto(string Email);