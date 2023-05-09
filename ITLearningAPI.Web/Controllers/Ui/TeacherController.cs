using ITLearning.Web.StaticAssets;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;


[ApiController]
[Route("[controller]")]
public class TeacherController : ControllerBase
{
    private readonly IStaticAssetsConfiguration _staticAssetsConfiguration;

    public TeacherController(IStaticAssetsConfiguration staticAssetsConfiguration)
    {
        _staticAssetsConfiguration = staticAssetsConfiguration ?? throw new ArgumentNullException(nameof(staticAssetsConfiguration));
    }

    [HttpGet]
    public async Task GetPage()
    {
        await HttpContext.Response.RespondWithStaticAsset(_staticAssetsConfiguration.DiskPath, "Teacher.html", "html", "text/html");
    }
}