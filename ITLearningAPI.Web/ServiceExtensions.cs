using ITLearning.Infrastructure.DataAccess.Common;
using ITLearning.Infrastructure.DataAccess.Mssql;

namespace ITLearningAPI.Web;

public static class ServiceExtensions
{
    public static IServiceCollection AddItLearningServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccessCommon(configuration);
        services.AddDataAccessMssql();
        return services;
    }
}