namespace ITLearningAPI.Web.Authorization;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApiAuthorizationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var authorizationSettings = AuthorizationSettings.GetFromConfiguration(configuration);
        services.AddSingleton<IAuthorizationSettings>(authorizationSettings);
        
        return services;
    }

}