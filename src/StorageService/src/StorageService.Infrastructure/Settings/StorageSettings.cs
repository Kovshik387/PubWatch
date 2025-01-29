namespace StorageService.Infrastructure.Settings;

public record StorageSettings
{
    /// <summary>
    /// Прокси
    /// </summary>
    public string Proxy { get; set; } = string.Empty;
    /// <summary>
    /// Порт
    /// </summary>
    public int ProxyPort { get; set; } = 9000;
    /// <summary>
    /// Подключение к Minio
    /// </summary>
    public string EndPoint { get; set; } = string.Empty;
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string AccessKey {  get; set; } = string.Empty;
    /// <summary>
    /// Секрет 
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;
    /// <summary>
    /// Включение SSL подключения
    /// </summary>
    public bool Ssl {  get; set; } = false;
}