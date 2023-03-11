using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITLearning.Infrastructure.DataAccess.Common;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDataAccessCommon(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConfiguration = DatabaseConfiguration.GetFromConfiguration(configuration);
        services.AddSingleton<IDatabaseConfiguration>(databaseConfiguration);
        services.AddSingleton<IDatabaseConnector, DatabaseConnector>();
        return services;
    }
}