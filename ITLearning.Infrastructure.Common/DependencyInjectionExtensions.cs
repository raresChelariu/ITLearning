using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITLearning.Infrastructure.Common;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructureCommon(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDatabaseConfiguration>(_ => DatabaseConfiguration.GetFromConfiguration(configuration));

        return services;
    }
}