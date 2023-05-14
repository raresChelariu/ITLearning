using System.Collections.Immutable;
using System.Net;
using System.Text;
using ITLearning.Web.StaticAssets.Configuration;
using ITLearning.Web.StaticAssets.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace ITLearning.Web.StaticAssets.Services;

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
            return;
        }
        var extension = receivedPath[(extensionIndex + 1)..];
        var isStaticTypeDefined = _staticTypesMapping.TryGetValue(extension, out var staticType);
        if (!isStaticTypeDefined)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        var fileDiskPath = Path.Combine(_staticAssetsConfiguration.RootDiskPath, staticType.DiskFolder, receivedPath);
        if (!File.Exists(fileDiskPath))
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        response.Headers[HeaderNames.ContentType] = staticType.ContentType;

        var contents = await File.ReadAllTextAsync(fileDiskPath);
        await response.Body.WriteAsync(Encoding.UTF8.GetBytes(contents));
    }

    private static IImmutableDictionary<string, StaticAssetType> BuildMappingDictionary(IEnumerable<StaticAssetType> staticAssetTypes)
    {
        return staticAssetTypes.ToImmutableDictionary(x => x.FileExtension, x => x);
    }
}