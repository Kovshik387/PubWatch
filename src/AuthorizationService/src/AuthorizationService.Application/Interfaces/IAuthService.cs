using AuthorizationService.Application.Dto;

namespace AuthorizationService.Application.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Аутентификация в существующий аккаунт
    /// </summary>
    /// <param name="model">Модель авторизации</param>
    /// <returns></returns>
    public Task<AuthDto> SignInAsync(SignInDto model);
    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    /// <param name="model">Модель регистрации</param>
    /// <returns></returns>
    public Task<AuthDto> SignUpAsync(SignUpDto model);
    /// <summary>
    /// Проверка на наличие пользователей в БД 
    /// </summary>
    /// <returns></returns>
    Task<bool> IsEmptyAsync();
    /// <summary>
    /// Выдача нового access-токена
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<AuthDto> GetAccessTokenAsync(RefreshDto request);
    /// <summary>
    /// Удаление refresh-токена у пользователя
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public Task<bool> Logout(string refreshToken);
    /// <summary>
    /// Удаление всех refresh-токенов отличные от пользовательского на текущем устройстве
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    public Task<bool> LogoutAll(string refreshToken);
}