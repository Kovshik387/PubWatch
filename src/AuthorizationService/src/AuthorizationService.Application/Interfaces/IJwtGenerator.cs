using AuthorizationService.Application.Dto;

namespace AuthorizationService.Application.Interfaces;

public interface IJwtGenerator
{
    /// <summary>
    /// Генерация jwt токена на основе id пользователя
    /// </summary>
    /// <param name="guid">Id пользователя</param>
    /// <returns></returns>
    public AuthDto GenerateJwtToken(Guid guid);
    /// <summary>
    /// Получение пользователя по refresh токену 
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public string? GetUserByRefreshToken(string refreshToken);
    /// <summary>
    /// Получение времени жизни токена
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public string? GetExpireTime(string refreshToken);
}