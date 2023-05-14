using ITLearning.Web.StaticAssets.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;


[ApiController]
[Route("[controller]")]
public class TeacherController : ControllerBase
{
    private readonly IStaticAssetResponseService _staticAssetResponseService;

    public TeacherController(IStaticAssetResponseService staticAssetResponseService)
    {
        _staticAssetResponseService = staticAssetResponseService ?? throw new ArgumentNullException(nameof(staticAssetResponseService));
    }

    [HttpGet]
    [Authorize(Roles = "Administrator,Teacher")]
    public async Task GetPage()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "Teacher.html");
    }
}