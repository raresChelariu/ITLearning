using System.Security.Claims;
using ITLearningAPI.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;


[ApiController]
public class IndexController : ControllerBase
{
    private readonly IStaticAssetResponseService _staticAssetResponseService;

    public IndexController(IStaticAssetResponseService staticAssetResponseService)
    {
        _staticAssetResponseService = staticAssetResponseService ?? throw new ArgumentNullException(nameof(staticAssetResponseService));
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
        await _staticAssetResponseService.RespondWithStaticAsset(response, "Index.html");
    }
}