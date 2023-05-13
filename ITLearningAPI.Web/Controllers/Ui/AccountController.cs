using ITLearning.Web.StaticAssets;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IStaticAssetsConfiguration _staticAssetsConfiguration;

    public AccountController(IStaticAssetsConfiguration staticAssetsConfiguration)
    {
        _staticAssetsConfiguration = staticAssetsConfiguration ?? throw new ArgumentNullException(nameof(staticAssetsConfiguration));
    }

    [HttpGet]
    [Route("AccessDenied")]
    public async Task<IActionResult> GetAccessDeniedPage()
    {
        await HttpContext.Response.RespondWithStaticAsset(_staticAssetsConfiguration.DiskPath, "AccessDenied.html");
        return Ok();
    }
}