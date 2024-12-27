using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthorizationService.Application.Dto;
using AuthorizationService.Application.Interfaces;
using AuthorizationService.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationService.Application.Services;

public class JwtGenerator : IJwtGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<JwtGenerator> _logger;
    
    public JwtGenerator(IOptions<JwtSettings> jwtSetting, ILogger<JwtGenerator> logger)
    {
        _jwtSettings = jwtSetting.Value;
        _logger = logger;
    }

    public AuthDto GenerateJwtToken(Guid guid)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>() { new Claim(ClaimTypes.Name, guid.ToString()) };

        var accessTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenLifetimeMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(_jwtSettings.SymmetricSecurityKeyAccess,
                SecurityAlgorithms.HmacSha256),
        };

        var refreshTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeDays),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(_jwtSettings.SymmetricSecurityKeyRefresh,
                SecurityAlgorithms.HmacSha256Signature),
        };

        return new AuthDto(guid,
            tokenHandler.WriteToken(tokenHandler.CreateToken(accessTokenDescriptor)),
            tokenHandler.WriteToken(tokenHandler.CreateToken(refreshTokenDescriptor)));
    }

    public string? GetExpireTime(string refreshToken)
    {
        var claims = GetClaimsFromToken(refreshToken);

        return claims?.Claims.First(x => x.Type.Equals("exp")).Value;
    }

    public string? GetUserByRefreshToken(string refreshToken)
    {
        var claims = GetClaimsFromToken(refreshToken);

        return claims?.Claims.First(x => x.Type.Equals(ClaimTypes.Name)).Value;
    }

    private ClaimsPrincipal? GetClaimsFromToken(string refreshToken)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _jwtSettings.SymmetricSecurityKeyRefresh
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            return tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out var validatedToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }
}