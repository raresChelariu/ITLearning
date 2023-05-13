using ITLearning.Web.StaticAssets;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;

[ApiController]
public class LogoutController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IStaticAssetsConfiguration _staticAssetsConfiguration;

    public LogoutController(IHttpClientFactory httpClientFactory, IStaticAssetsConfiguration staticAssetsConfiguration)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _staticAssetsConfiguration = staticAssetsConfiguration ?? throw new ArgumentNullException(nameof(staticAssetsConfiguration));
    }

    [HttpGet]
    [Route("/logout")]
    public async Task<IActionResult> GetLogoutPage()
    {
        var httpClient = _httpClientFactory.CreateClient("Internal");
        await httpClient.PostAsync("/api/user/logout", new StringContent(string.Empty));
        HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
        await HttpContext.Response.RespondWithStaticAsset(_staticAssetsConfiguration.DiskPath,
            receivedPath: "Logout.html");
        return Ok();
    }
}