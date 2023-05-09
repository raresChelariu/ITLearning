using ITLearning.Infrastructure.DataAccess.Common;
using ITLearning.Infrastructure.DataAccess.Mssql;
using ITLearning.Web.StaticAssets;
using ITLearningAPI.Web.Authorization;

namespace ITLearningAPI.Web;

public static class ServiceExtensions
{
    public static IServiceCollection AddItLearningServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccessCommon(configuration);
        services.AddDataAccessMssql();
        services.AddApiAuthorizationSettings(configuration);
        services.AddStaticAssets(configuration);
        return services;
    }
}