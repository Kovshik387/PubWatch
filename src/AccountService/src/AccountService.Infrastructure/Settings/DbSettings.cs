namespace AccountService.Infrastructure.Settings;

public record DbSettings(string ConnectionString, bool DetailedLog);