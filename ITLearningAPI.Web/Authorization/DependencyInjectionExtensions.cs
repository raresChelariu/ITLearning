using Microsoft.AspNetCore.Authentication;

namespace ITLearningAPI.Web.Authorization;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAuthorizationDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = AuthorizationSettings.GetFromConfiguration(configuration);
        return services
            .AddSingleton<IJwtService, JwtService>()
            .AddSingleton<IMixedAuthenticationService, MixedAuthenticationService>()
            .AddSingleton<IAuthorizationSettings>(_ => settings);
    }

    public static AuthenticationBuilder AddMixedJwtCookieAuthentication(this AuthenticationBuilder builder)
    {
        builder.AddScheme<AuthenticationSchemeOptions, MixedAuthenticationHandler>(
            AuthorizationSchemas.MixedSchema, _ => {}
        );
        return builder;
    }
}