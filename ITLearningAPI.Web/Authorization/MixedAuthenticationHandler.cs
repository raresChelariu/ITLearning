using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ITLearningAPI.Web.Authorization;

public class MixedAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IMixedAuthenticationService _authenticationService;

    public MixedAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock, 
        IMixedAuthenticationService authenticationService) 
        : base(options, logger, encoder, clock)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var cookieAuthToken = Request.Cookies["AuthToken"];
        return await _authenticationService.GetAuthenticationResultFromHeaders(authorizationHeader, cookieAuthToken, Scheme.Name);
    }
}