using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationService.Application.Settings;

public class JwtSettings
{
    /// <summary>
    /// Имя издателя
    /// </summary>
    public string Issuer { get; set; } = null!;
    /// <summary>
    /// Имя получателя
    /// </summary>
    public string Audience { get; set; } = null!;
    /// <summary>
    /// Секрет для access-токена
    /// </summary>
    public string SecretAccess { get; set; } = null!;
    /// <summary>
    /// Секрет для refresh-токена
    /// </summary>
    public string SecretRefresh { get; set; } = null!;
    /// <summary>
    /// Время жизни Access-токена в минутах
    /// </summary>
    public int AccessTokenLifetimeMinutes { get; set; }
    /// <summary>
    /// Время жизни Refresh-токена в днях
    /// </summary>
    public int RefreshTokenLifetimeDays { get; set; }
    /// <summary>
    /// Симметричный ключ для подписи Access-токенов
    /// </summary>
    public SymmetricSecurityKey SymmetricSecurityKeyAccess => new(Encoding.UTF8.GetBytes(SecretAccess));
    /// <summary>
    /// Симметричный ключ для подписи Refresh-токенов
    /// </summary>
    public SymmetricSecurityKey SymmetricSecurityKeyRefresh => new(Encoding.UTF8.GetBytes(SecretRefresh));
}