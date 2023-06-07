using ITLearningAPI.Web.Authorization;
using ITLearningAPI.Web.Interfaces;
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

    [Authorize(Policy = AuthorizationPolicies.AdminOrTeacher)]
    [HttpGet]
    public async Task GetPage()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "Teacher.html");
    }

    [Authorize(Policy = AuthorizationPolicies.AdminOrTeacher)]
    [HttpGet("courses/mine")]
    public async Task GetTeacherCoursesPage()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "TeacherCoursesMine.html");
    }

    [Authorize(Policy = AuthorizationPolicies.AdminOrTeacher)]
    [HttpGet("courses/all")]
    public async Task GetAllCoursesPage()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "TeacherCoursesAll.html");
    }
    
    [Authorize(Policy = AuthorizationPolicies.AdminOrTeacher)]
    [HttpGet("/teacher/course/create")]
    public async Task GetCourseCreatePage()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "CourseCreate.html");
    }
}

