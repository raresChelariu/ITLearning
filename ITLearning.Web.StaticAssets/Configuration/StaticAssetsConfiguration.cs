using Microsoft.Extensions.Configuration;

namespace ITLearning.Web.StaticAssets.Configuration;

internal class StaticAssetsConfiguration : IStaticAssetsConfiguration
{
    public string RootDiskPath { get; set; }
    public IEnumerable<StaticAssetType> StaticAssetTypes { get; set; }

    public static StaticAssetsConfiguration GetFromConfiguration(IConfiguration configuration)
    {
        var settings = configuration.GetSection(IStaticAssetsConfiguration.ConfigurationKey)
            .Get<StaticAssetsConfiguration>();

        return settings;
    }
}