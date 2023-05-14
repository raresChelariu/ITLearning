using ITLearning.Infrastructure.DataAccess.Common;
using ITLearning.Infrastructure.DataAccess.Mssql;
using ITLearning.Utils;
using ITLearning.Web.StaticAssets;

namespace ITLearningAPI.Web;

public static class ServiceExtensions
{
    public static IServiceCollection AddItLearningServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccessCommon(configuration)
            .AddDataAccessMssql()
            .AddStaticAssets(configuration)
            .AddUtils();

        return services;
    }
}