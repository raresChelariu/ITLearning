using ITLearning.TypeGuards;

namespace ITLearningAPI.Web.Authorization;

internal class AuthorizationSettings : IAuthorizationSettings
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int TokenTimeoutMinutes { get; set; }

    public static AuthorizationSettings GetFromConfiguration(IConfiguration configuration)
    {
        var settings = configuration.GetSection(IAuthorizationSettings.ConfigurationKey).Get<AuthorizationSettings>();

        TypeGuard.ThrowIfStringIsNullOrWhitespace(settings.Secret);
        TypeGuard.ThrowIfStringIsNullOrWhitespace(settings.Audience);
        TypeGuard.ThrowIfStringIsNullOrWhitespace(settings.Issuer);
        TypeGuard.ThrowIfZeroOrNegative(settings.TokenTimeoutMinutes);

        return settings;
    }
}