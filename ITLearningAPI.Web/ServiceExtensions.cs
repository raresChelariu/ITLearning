using ITLearning.Infrastructure.Common;

namespace ITLearningAPI.Web;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureCommon(configuration);

        return services;
    }
}