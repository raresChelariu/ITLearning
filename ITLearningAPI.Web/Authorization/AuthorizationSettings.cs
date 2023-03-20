using ITLearning.TypeGuards;

namespace ITLearningAPI.Web.Authorization;

internal class AuthorizationSettings : IAuthorizationSettings
{
    private const string ConfigurationKey = "authorization";
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }

    public static AuthorizationSettings GetFromConfiguration(IConfiguration configuration)
    {
        var settings = configuration.GetSection(ConfigurationKey).Get<AuthorizationSettings>();
        TypeGuard.ThrowIfStringIsNullOrWhitespace(settings.Secret);
        TypeGuard.ThrowIfStringIsNullOrWhitespace(settings.Audience);
        TypeGuard.ThrowIfStringIsNullOrWhitespace(settings.Issuer);
        return settings;
    }
}