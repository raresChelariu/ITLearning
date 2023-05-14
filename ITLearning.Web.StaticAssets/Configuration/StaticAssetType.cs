namespace ITLearning.Web.StaticAssets.Configuration;

public record StaticAssetType
{
    public string FileExtension { get; init; }
    public string ContentType { get; init; }

    public string DiskFolder { get; init; }

}