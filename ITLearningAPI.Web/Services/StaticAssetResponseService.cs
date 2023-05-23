using System.Collections.Immutable;
using ITLearning.Web.StaticAssets;
using System.Net;
using System.Text;
using ITLearningAPI.Web.Interfaces;

namespace ITLearningAPI.Web.Services;

public class StaticAssetResponseService : IStaticAssetResponseService
{
    private readonly IStaticAssetsConfiguration _staticAssetsConfiguration;
    private readonly IImmutableDictionary<string, StaticAssetType> _staticTypesMapping;

    public StaticAssetResponseService(IStaticAssetsConfiguration staticAssetsConfiguration)
    {
        _staticAssetsConfiguration = staticAssetsConfiguration ?? throw new ArgumentNullException(nameof(staticAssetsConfiguration));
        _staticTypesMapping = BuildMappingDictionary(staticAssetsConfiguration.StaticAssetTypes);
    }
    
    public async Task RespondWithStaticAsset(HttpResponse response, string receivedPath)
    {
        var extensionIndex = receivedPath.LastIndexOf(".", StringComparison.InvariantCultureIgnoreCase);
        if (extensionIndex == -1)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            await response.CompleteAsync();
            return;
        }
        var extension = receivedPath[(extensionIndex + 1)..];
        var isStaticTypeDefined = _staticTypesMapping.TryGetValue(extension, out var staticType);
        if (!isStaticTypeDefined)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            await response.CompleteAsync();
            return;
        } 

        var fileDiskPath = Path.Combine(_staticAssetsConfiguration.RootDiskPath, staticType.DiskFolder, receivedPath);
        if (!File.Exists(fileDiskPath))
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            await response.CompleteAsync();
            return;
        }
        
        var contents = await File.ReadAllTextAsync(fileDiskPath);
        response.StatusCode = (int)HttpStatusCode.OK;
        await response.Body.WriteAsync(Encoding.UTF8.GetBytes(contents));
        await response.CompleteAsync();
    }

    private static IImmutableDictionary<string, StaticAssetType> BuildMappingDictionary(IEnumerable<StaticAssetType> staticAssetTypes)
    {
        return staticAssetTypes.ToImmutableDictionary(x => x.FileExtension, x => x);
    }
}