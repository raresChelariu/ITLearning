using ITLearningAPI.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;

[ApiController]
public class StaticAssetsController : ControllerBase
{
    private readonly IStaticAssetResponseService _staticAssetResponseService;

    public StaticAssetsController(IStaticAssetResponseService staticAssetResponseService)
    {
        _staticAssetResponseService = staticAssetResponseService ?? throw new ArgumentNullException(nameof(staticAssetResponseService));
    }
    
    [HttpGet]
    [Route("/css/{*path}")]
    public async Task GetCss(string path)
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, path);
    }

    [HttpGet]
    [Route("/js/{*path}")]
    public async Task GetJs(string path)
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, path);
    }

    [HttpGet]
    [Route("/images/{*path}")]
    public async Task GetImages(string path)
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, path);
    }

}