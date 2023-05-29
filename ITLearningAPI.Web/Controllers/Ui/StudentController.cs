using ITLearningAPI.Web.Authorization;
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

    [Authorize(Policy = AuthorizationPolicies.AdminOrStudent)]
    [HttpGet]
    public async Task GetPage()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "Student.html");
    }

    [Authorize(Policy = AuthorizationPolicies.AdminOrStudent)]
    [HttpGet("courses/all")]
    public async Task GetAllCourses()
    {
        await _staticAssetResponseService.RespondWithStaticAsset(Response, "CoursesAll.html");
    }
}