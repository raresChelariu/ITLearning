namespace ITLearningAPI.Web.Authorization;

public interface IAuthorizationSettings
{
    const string ConfigurationKey = "authorization";

    public string Secret { get; }
    public string Issuer { get; }
    public string Audience { get; }
    public int TokenTimeoutMinutes { get; }
}