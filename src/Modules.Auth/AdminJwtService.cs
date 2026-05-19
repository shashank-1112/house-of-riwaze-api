using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Modules.Auth;

public sealed class AdminJwtService
{
    private readonly IConfiguration _configuration;

    public AdminJwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AdminLoginResponse CreateToken(string username)
    {
        var issuer = GetRequiredConfiguration("Jwt:Issuer");
        var audience = GetRequiredConfiguration("Jwt:Audience");
        var key = GetSigningKey();
        var expiryMinutes = GetExpiryMinutes();
        var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, AdminAuthorization.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AdminLoginResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAtUtc = expiresAt
        };
    }

    public bool ValidateCredentials(AdminLoginRequest request)
    {
        var username = _configuration["AdminCredentials:Username"];
        var password = _configuration["AdminCredentials:Password"];

        return !string.IsNullOrWhiteSpace(username)
            && !string.IsNullOrWhiteSpace(password)
            && string.Equals(request.Username, username, StringComparison.Ordinal)
            && string.Equals(request.Password, password, StringComparison.Ordinal);
    }

    internal SymmetricSecurityKey GetSigningKey()
    {
        var key = GetRequiredConfiguration("Jwt:Key");
        var bytes = Encoding.UTF8.GetBytes(key);

        if (bytes.Length < 32)
        {
            throw new InvalidOperationException("Jwt:Key must be at least 32 bytes long.");
        }

        return new SymmetricSecurityKey(bytes);
    }

    private int GetExpiryMinutes()
    {
        var value = _configuration["Jwt:ExpiryMinutes"];

        return int.TryParse(value, out var expiryMinutes) && expiryMinutes > 0
            ? expiryMinutes
            : 120;
    }

    private string GetRequiredConfiguration(string key)
    {
        var value = _configuration[key];

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"{key} is required.");
        }

        return value;
    }
}
