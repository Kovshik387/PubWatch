using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationService.Domain.Settings;

public class JwtSettings
{
    /// <summary>
    /// Имя издателя
    /// </summary>
    public required string Issuer { get; set; } = null!;
    /// <summary>
    /// Имя получателя
    /// </summary>
    public required string Audience { get; set; } = null!;
    /// <summary>
    /// Секрет для access-токена
    /// </summary>
    public required string SecretAccess { get; set; } = null!;
    /// <summary>
    /// Секрет для refresh-токена
    /// </summary>
    public required string SecretRefresh { get; set; } = null!;
    /// <summary>
    /// Время жизни Access-токена в минутах
    /// </summary>
    public required int AccessTokenLifetimeMinutes { get; set; }
    /// <summary>
    /// Время жизни Refresh-токена в днях
    /// </summary>
    public required int RefreshTokenLifetimeDays { get; set; }
    /// <summary>
    /// Симметричный ключ для подписи Access-токенов
    /// </summary>
    public SymmetricSecurityKey SymmetricSecurityKeyAccess => new(Encoding.UTF8.GetBytes(SecretAccess));
    /// <summary>
    /// Симметричный ключ для подписи Refresh-токенов
    /// </summary>
    public SymmetricSecurityKey SymmetricSecurityKeyRefresh => new(Encoding.UTF8.GetBytes(SecretRefresh));
}