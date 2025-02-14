namespace BackgroundDailyService.Infrastructure.Settings;

public class EndPoints
{
    public required string HttpUrl { get; init; }
    public required string GrpcUrlSend { get; init; }
    public required string GrpcUrlGet { get; init; }
}