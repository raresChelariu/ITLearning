using ITLearning.Web.StaticAssets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace ITLearningAPI.Web.Controllers.Ui;

[ApiController]
public class StaticAssetsController : ControllerBase
{
    private readonly IStaticAssetsConfiguration _staticAssetsConfiguration;

    public StaticAssetsController(IStaticAssetsConfiguration staticAssetsConfiguration)
    {
        _staticAssetsConfiguration = staticAssetsConfiguration ?? throw new ArgumentNullException(nameof(staticAssetsConfiguration));
    }
    

    [HttpGet]
    [Route("/css/{*path}")]
    [OutputCache]
    public async Task GetCss(string path)
    {
        await HttpContext.Response.RespondWithStaticAsset(_staticAssetsConfiguration.DiskPath, path, "css", "text/css");
    }

    [HttpGet]
    [Route("/js/{*path}")]
    [OutputCache]
    public async Task GetJs(string path)
    {
        await HttpContext.Response.RespondWithStaticAsset(_staticAssetsConfiguration.DiskPath,
            receivedPath: path,
            targetFolderName: "js",
            contentType: "text/javascript");
    }

    [HttpGet]
    [Route("/images/{*path}")]
    [OutputCache]
    public async Task GetImages(string path)
    {
        await HttpContext.Response.RespondWithStaticAsset(_staticAssetsConfiguration.DiskPath,
            receivedPath: path,
            targetFolderName: "image",
            contentType: "image/png");
    }

}