using ITLearning.Infrastructure.DataAccess.Common;
using ITLearning.Infrastructure.DataAccess.Mssql;
using ITLearningAPI.Web.Authorization;

namespace ITLearningAPI.Web;

public static class ServiceExtensions
{
    public static IServiceCollection AddItLearningServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccessCommon(configuration);
        services.AddDataAccessMssql();
        services.AddApiAuthorizationSettings(configuration);
        return services;
    }
}