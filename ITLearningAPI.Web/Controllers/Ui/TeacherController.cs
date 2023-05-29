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
    public async Task GetTeacherCourses()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "CoursesMine.html");
    }

    [Authorize(Policy = AuthorizationPolicies.AdminOrTeacher)]
    [HttpGet("courses/all")]
    public async Task GetAllCourses()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "CoursesAll.html");
    }
}

