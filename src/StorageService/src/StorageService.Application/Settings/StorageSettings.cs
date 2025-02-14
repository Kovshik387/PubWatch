namespace StorageService.Application.Settings;

public record StorageSettings
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string AccessKey {  get; init; } = string.Empty;
    /// <summary>
    /// Секрет 
    /// </summary>
    public string SecretKey { get; init; } = string.Empty;
    /// <summary>
    /// Регион
    /// </summary>
    public string Region { get; init; } = string.Empty;
    /// <summary>
    /// Подключение к S3
    /// </summary>
    public string EndPoint { get; set; } = string.Empty;
    /// <summary>
    /// Внешний url
    /// </summary>
    public string PublicEndPoint { get; set; } = string.Empty;
    /// <summary>
    /// Приватрный url
    /// </summary>
    public string PrivateEndPoint { get; set; } = string.Empty;
}