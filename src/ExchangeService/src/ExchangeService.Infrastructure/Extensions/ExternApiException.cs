namespace ExchangeService.Infrastructure.Extensions;

public class ExternApiException : Exception
{
    public ExternApiException(string message) : base(message) { }
}