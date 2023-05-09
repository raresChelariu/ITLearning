using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace ITLearning.Web.StaticAssets;

public static class StaticAssetExtensions
{
    public static async Task RespondWithStaticAsset(this HttpResponse response,
        string rootDiskPath, string receivedPath,
        string targetFolderName, string contentType)
    {

        var fileDiskPath = Path.Combine(rootDiskPath, targetFolderName, receivedPath);
        if (!File.Exists(fileDiskPath))
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        response.Headers[HeaderNames.ContentType] = contentType;

        var contents = await File.ReadAllTextAsync(fileDiskPath);
        await response.Body.WriteAsync(Encoding.UTF8.GetBytes(contents));
    }
}