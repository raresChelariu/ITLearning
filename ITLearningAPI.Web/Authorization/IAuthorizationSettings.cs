namespace ITLearningAPI.Web.Authorization;

public interface IAuthorizationSettings
{
    public string Secret { get; }
    public string Issuer { get; }
    public string Audience { get; }
}