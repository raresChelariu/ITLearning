using Microsoft.AspNetCore.Authentication;

namespace ITLearningAPI.Web.Authorization;

public interface IMixedAuthenticationService
{
    Task<AuthenticateResult> GetAuthenticationResultFromHeaders(string authorizationHeader, string cookieAuthToken, string schemeName);
}