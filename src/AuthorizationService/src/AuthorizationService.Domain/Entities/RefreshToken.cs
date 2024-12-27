namespace AuthorizationService.Domain.Entities;

public partial class RefreshToken
{
    public Guid Id { get; init; }
    
    public string Token { get; set; } = string.Empty;
    
    public string Device { get; set; } = string.Empty;
    
    public Guid? Idaccount { get; set; }
    
    public virtual Account? IdAccountNavigation { get; set; }
}