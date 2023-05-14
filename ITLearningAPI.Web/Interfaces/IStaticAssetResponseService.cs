namespace ITLearningAPI.Web.Interfaces;

public interface IStaticAssetResponseService
{
    Task RespondWithStaticAsset(HttpResponse response, string receivedPath);
}