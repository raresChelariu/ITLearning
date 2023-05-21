using ITLearningAPI.Web.Interfaces;
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

    [Authorize(Policy = "AdminOrStudent")]
    [HttpGet]
    public async Task GetPage()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "Student.html");
    }

    [Authorize(Policy = "AdminOrStudent")]
    [HttpGet("courses")]
    public async Task GetStudentCourses()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "CoursesAll.html");
    }
}