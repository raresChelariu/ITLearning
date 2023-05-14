namespace ITLearning.Web.StaticAssets;

public interface IStaticAssetsConfiguration
{
    const string ConfigurationKey = "staticAssets";

    string RootDiskPath { get; }

    public IEnumerable<StaticAssetType> StaticAssetTypes { get; }
}