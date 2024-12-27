using Domain.Dto;

namespace Application.Interfaces;

public interface IJwtGenerator
{
    /// <summary>
    /// Генерирует пару refresh и access токена на основе id пользователя
    /// </summary>
    /// <param name="guid">Guid пользователя</param>
    /// <returns></returns>
    public AuthDto GenerateJwtToken(Guid guid);
    /// <summary>
    /// Возвращает время действия токена
    /// </summary>
    /// <param name="refreshToken">Refresh токен</param>
    /// <returns></returns>
    public string? GetExpireTime(string refreshToken);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="refreshToken">Refresh токен</param>
    /// <returns></returns>
    public string? GetUserByRefreshToken(string refreshToken);
}