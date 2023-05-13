using System.Security.Claims;
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
    public async Task<IActionResult> GetDefaultPage()
    {
        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            await DirectToSignIn(HttpContext.Response);
            return Ok();
        }

        var claims = HttpContext.User.Claims;
        var roleClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
        var roleValue = roleClaim?.Value;
        if (string.IsNullOrEmpty(roleValue))
        {
            await DirectToSignIn(HttpContext.Response);
            return Ok();
        }

        roleValue = roleValue.ToLowerInvariant();
        switch (roleValue)
        {
            case "teacher":
                return RedirectPreserveMethod("https://localhost:7032/teacher");
            case "student":
                return RedirectPermanent("https://localhost:7032/student");
            default:
                await DirectToSignIn(HttpContext.Response);
                return Ok();
        }
    }

    private async Task DirectToSignIn(HttpResponse response)
    {
        await response.RespondWithStaticAsset(
            rootDiskPath: _staticAssetsConfiguration.DiskPath,
            receivedPath: "Index.html",
            targetFolderName: "html",
            contentType: "text/html");
    }
}