using ITLearningAPI.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;

[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase
{
    private readonly IStaticAssetResponseService _staticAssetResponseService;

    public CourseController(IStaticAssetResponseService staticAssetResponseService)
    {
        _staticAssetResponseService = staticAssetResponseService 
                                      ?? throw new ArgumentNullException(nameof(staticAssetResponseService));
    }

    [HttpGet]
    [Route("create")]
    public async Task GetCourseCreate()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "CourseCreate.html");
    }
}