namespace ITLearning.Web.StaticAssets.Configuration;

public interface IStaticAssetsConfiguration
{
    const string ConfigurationKey = "StaticAssets";

    string RootDiskPath { get; }

    public IEnumerable<StaticAssetType> StaticAssetTypes { get; }
}