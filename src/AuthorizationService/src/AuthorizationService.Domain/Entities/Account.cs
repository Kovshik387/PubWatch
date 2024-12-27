namespace AuthorizationService.Domain.Entities;

public partial class Account
{
    public Guid Id { get; init; }
    
    public required string Email { get; set; } = null!;
    
    public required string EmailNormalized { get; set; } = null!;

    public required string PasswordHash { get; set; } = null!;
    
    public virtual required ICollection<RefreshToken> Refreshes { get; set; }
}