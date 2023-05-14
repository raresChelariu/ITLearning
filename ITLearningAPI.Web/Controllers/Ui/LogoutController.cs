using ITLearning.Web.StaticAssets.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;

[ApiController]
public class LogoutController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IStaticAssetResponseService _staticAssetResponseService;

    public LogoutController(IHttpClientFactory httpClientFactory, IStaticAssetResponseService staticAssetResponseService)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _staticAssetResponseService = staticAssetResponseService ?? throw new ArgumentNullException(nameof(staticAssetResponseService));
    }

    [HttpGet]
    [Route("/logout")]
    public async Task<IActionResult> GetLogoutPage()
    {
        var httpClient = _httpClientFactory.CreateClient("Internal");
        await httpClient.PostAsync("/api/user/logout", new StringContent(string.Empty));
        HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "Logout.html");
        return Ok();
    }
}