using ITLearning.Web.StaticAssets;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;


[ApiController]
public class IndexController : ControllerBase
{
    private readonly IStaticAssetsConfiguration _staticAssetsConfiguration;

    public IndexController(IStaticAssetsConfiguration staticAssetsConfiguration)
    {
        _staticAssetsConfiguration = staticAssetsConfiguration ?? throw new ArgumentNullException(nameof(staticAssetsConfiguration));
    }

    [HttpGet]
    [Route("/")]
    public async Task GetDefaultPage()
    {
        await HttpContext.Response.RespondWithStaticAsset(
            rootDiskPath: _staticAssetsConfiguration.DiskPath,
            receivedPath: "Index.html",
            targetFolderName: "html",
            contentType: "text/html");
    }


}