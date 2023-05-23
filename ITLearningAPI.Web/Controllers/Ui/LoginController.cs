using ITLearningAPI.Web.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IStaticAssetResponseService _staticAssetResponseService;

    public LoginController(IStaticAssetResponseService staticAssetResponseService)
    {
        _staticAssetResponseService = staticAssetResponseService ?? throw new ArgumentNullException(nameof(staticAssetResponseService));
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task ServeLoginPage()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "Index.html");
    }
}