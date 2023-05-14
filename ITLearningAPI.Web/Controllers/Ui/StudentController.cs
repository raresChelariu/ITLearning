using ITLearning.Web.StaticAssets.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{

    private readonly IStaticAssetResponseService _staticAssetResponseService;

    public StudentController(IStaticAssetResponseService staticAssetResponseService)
    {
        _staticAssetResponseService = staticAssetResponseService ?? throw new ArgumentNullException(nameof(staticAssetResponseService));
    }

    [HttpGet]
    [Authorize(Roles = "Administrator,Student")]
    public async Task GetPage()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "Student.html");
    }

    [HttpGet("courses")]
    [Authorize(Roles = "Administrator,Student")]
    public async Task GetStudentCourses()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "CoursesAll.html");
    }
}