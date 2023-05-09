using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITLearning.Web.StaticAssets;

public static class StaticAssetsDiEx
{
    public static IServiceCollection AddStaticAssets(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = StaticAssetsConfiguration.GetFromConfiguration(configuration);

        services.AddSingleton<IStaticAssetsConfiguration>(_ => settings);

        return services;
    }
}