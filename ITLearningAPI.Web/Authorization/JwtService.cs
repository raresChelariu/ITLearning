using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ITLearning.Utils.Contracts;

namespace ITLearningAPI.Web.Authorization;

public class JwtService : IJwtService
{
    private readonly IAuthorizationSettings _authorizationSettings;
    private readonly IDateTimeProvider _dateTimeProvider;

    public JwtService(IAuthorizationSettings authorizationSettings, IDateTimeProvider dateTimeProvider)
    {
        _authorizationSettings = authorizationSettings ?? throw new ArgumentNullException(nameof(authorizationSettings));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    public string CreateJwtToken(ICollection<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authorizationSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = _dateTimeProvider.UtcNow.AddMinutes(_authorizationSettings.TokenTimeoutMinutes);

        claims.Add(new Claim(ClaimTypes.Expiration, expiration.Ticks.ToString()));

        var token = new JwtSecurityToken(
            issuer: _authorizationSettings.Issuer,
            audience: _authorizationSettings.Audience,
            claims: claims,
            signingCredentials: credentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    public ICollection<Claim> GetClaims(string token)
    {
        var validator = new JwtSecurityTokenHandler();
        try
        {
            var validationParameters = GetValidationParameters();
            var principal = validator.ValidateToken(token, validationParameters, out _);
            var claims = principal.Claims.ToList();
            return claims;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private TokenValidationParameters GetValidationParameters()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authorizationSettings.Secret));

        var validationParameters = new TokenValidationParameters
        {
            ValidIssuer = _authorizationSettings.Issuer,
            IssuerSigningKey = key,
            ValidateIssuerSigningKey = true
        };
        return validationParameters;
    }
}