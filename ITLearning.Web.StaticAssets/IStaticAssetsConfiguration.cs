namespace ITLearning.Web.StaticAssets;

public interface IStaticAssetsConfiguration
{
    const string ConfigurationKey = "staticAssets";

    string DiskPath { get; }
}