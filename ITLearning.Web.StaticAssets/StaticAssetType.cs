namespace ITLearning.Web.StaticAssets;

public record StaticAssetType
{
    public string FileExtension { get; init; }
    public string ContentType { get; init; }

    public string DiskFolder { get; init; }
    
}