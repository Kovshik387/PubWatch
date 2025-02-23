using System.Net;
using System.Net.Mail;
using System.Text;
using MediatR;
using MessageService.Application.Interfaces;
using MessageService.Application.Settings;
using MessageService.Domain;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MessageService.Application.Features;

public record SendNotificationCommand(IReadOnlyCollection<string> Accounts) : IRequest;

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand>
{
    private readonly SmtpClient _smtpClient;
    private readonly ILogger<SendNotificationCommandHandler> _logger;
    private readonly IDistributedCache _cache;
    private readonly IServiceClient _client;
    
    private const string DateFormatString = "dd.MM.yyyy";
    
    private readonly JsonSerializerSettings _serializerSettings;
    private readonly EmailSettings _emailSettings;
    
    public SendNotificationCommandHandler(IOptions<EmailSettings> emailSettings,
        ILogger<SendNotificationCommandHandler> logger, IDistributedCache cache, IServiceClient client)
    {
        _logger = logger;
        _cache = cache;
        _client = client;
        _emailSettings = emailSettings.Value;
        
        _serializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = DateFormatString,
        };
        
        _smtpClient = new SmtpClient(_emailSettings.Provider)
        {
            Port = _emailSettings.Port,
            EnableSsl = true,
            Credentials = new NetworkCredential()
            {
                Password = _emailSettings.Password,
                UserName = _emailSettings.Email
            }
        };
    }

    public async Task Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1).ToString("dd.MM.yyyy");
        
        var currencies = await GetCurrenciesAsync(date);
        
        _logger.LogInformation($"{_emailSettings.Email}");
        
        if (currencies == null)
        {
            _logger.LogInformation($"No Currencies {date}");
            return;
        }

        foreach (var item in request.Accounts)
        {
            var sender = new MailAddress(_emailSettings.Email, $"Информация о валютах");

            var builder = new StringBuilder(GetHtmlBody(date));

            foreach (var currency in currencies)
            {
                builder.AppendLine($"<li>{currency.Name} : {currency.Value}</li>");
            }
            
            builder.AppendLine("</ul></div></body></html>"
            );

            var receiver = new MailAddress(item);

            var message = new MailMessage(sender, receiver)
            {
                IsBodyHtml = true,
                Subject = "Котировки валют",
                Body = builder.ToString(),
            };

            await _smtpClient.SendMailAsync(message, cancellationToken);
            _logger.LogInformation($"to {_emailSettings.Email}");
        }
    }

    private async Task<IReadOnlyCollection<Currency>?> GetCurrenciesAsync(string key)
    {
        var currenciesStr = await _cache.GetStringAsync(key);
        
        if (!string.IsNullOrEmpty(currenciesStr))
            return JsonConvert.DeserializeObject<Exchange>(currenciesStr, _serializerSettings)!.Volute;
        
        var response = await _client.GetCurrenciesAsync();
        
        await SetCurrenciesAsync(key,response);
        
        var currencies = JsonConvert.DeserializeObject<Exchange> 
            (response,_serializerSettings);

        return currencies?.Volute;
    }

    private async Task SetCurrenciesAsync(string key, string currencies)
    {
        await _cache.SetStringAsync(key, currencies, new DistributedCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromMinutes(5)
        });
    }
    
    #region Template

    private static string GetHtmlBody(string date)
    {
        return $@"
        <!DOCTYPE html>
        <html lang=""ru"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Котировки валют</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f9;
                    color: #333;
                    margin: 0;
                    padding: 20px;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #fff;
                    border-radius: 8px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                    padding: 20px;
                }}
                h1 {{
                    color: #8884d8;
                    text-align: center;
                    margin-bottom: 20px;
                }}
                p {{
                    font-size: 16px;
                    text-align: center;
                    color: #666;
                }}
                ul {{
                    list-style-type: none;
                    padding: 0;
                }}
                li {{
                    background-color: #f9f9f9;
                    margin: 10px 0;
                    padding: 10px;
                    border-left: 4px solid #8884d8;
                    border-radius: 4px;
                    font-size: 14px;
                }}
                li:hover {{
                    background-color: #f1f1f1;
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <h1>Данные от {date}</h1>
                <ul>";
    }

    #endregion
}