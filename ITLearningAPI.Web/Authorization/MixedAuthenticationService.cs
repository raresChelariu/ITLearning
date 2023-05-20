using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace ITLearningAPI.Web.Authorization;

public class MixedAuthenticationService : IMixedAuthenticationService
{
    private readonly IJwtService _jwtService;

    public MixedAuthenticationService(IJwtService jwtService)
    {
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
    }

    public Task<AuthenticateResult> GetAuthenticationResultFromHeaders(string authorizationHeader, string cookieAuthToken, string schemeName)
    {
        var token = GetToken(authorizationHeader, cookieAuthToken);
        if (token is null)
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing token"));
        }
        var claims = _jwtService.GetClaims(token);
        
        if (claims == null)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
        }

        var expiration = claims.FirstOrDefault(x => x.Type == ClaimTypes.Expiration)?.Value;
        var isValidTicksNumber = long.TryParse(expiration, out var expirationTicks);
        if (!isValidTicksNumber || DateTime.UtcNow.Ticks > expirationTicks)
        {
            return Task.FromResult(AuthenticateResult.Fail("Expired token"));
        }

        var identity = new ClaimsIdentity(claims, schemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, schemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
    
    private static string GetToken(string authorizationHeader, string cookieAuthToken)
    {
        var token = GetTokenFromAuthorizationHeader(authorizationHeader);
        if (token is null)
        {
            return null;
        }
        token = GetTokenFromCookies(cookieAuthToken);
        return token;
    }

    private static string GetTokenFromCookies(string cookieAuthToken)
    {
        return null;
    }

    private static string GetTokenFromAuthorizationHeader(string authHeader)
    {
        if (!authHeader.StartsWith("Bearer"))
        {
            return null;
        }
        var authHeaderParts = authHeader.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (authHeaderParts.Length != 2)
        {
            return null;
        }
        var token = authHeaderParts[1];

        return token;
    }
}