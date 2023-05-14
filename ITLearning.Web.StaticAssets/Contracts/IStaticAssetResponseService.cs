using Microsoft.AspNetCore.Http;

namespace ITLearning.Web.StaticAssets.Contracts;

public interface IStaticAssetResponseService
{
    Task RespondWithStaticAsset(HttpResponse response, string receivedPath);
}