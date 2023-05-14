using ITLearningAPI.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IStaticAssetResponseService _staticAssetResponseService;

    public AccountController(IStaticAssetResponseService staticAssetResponseService)
    {
        _staticAssetResponseService = staticAssetResponseService ?? throw new ArgumentNullException(nameof(staticAssetResponseService));
    }

    [HttpGet]
    [Route("AccessDenied")]
    public async Task<IActionResult> GetAccessDeniedPage()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "AccessDenied.html");
        return Ok();
    }
}