using Microsoft.Extensions.Configuration;

namespace ITLearning.Web.StaticAssets;

internal class StaticAssetsConfiguration : IStaticAssetsConfiguration
{
    public string DiskPath { get; set; }

    public static StaticAssetsConfiguration GetFromConfiguration(IConfiguration configuration)
    {
        var settings = configuration.GetSection(IStaticAssetsConfiguration.ConfigurationKey)
            .Get<StaticAssetsConfiguration>();

        return settings;
    }
}