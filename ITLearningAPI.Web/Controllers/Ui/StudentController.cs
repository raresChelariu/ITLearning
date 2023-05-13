using ITLearning.Web.StaticAssets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStaticAssetsConfiguration _staticAssetsConfiguration;

    public StudentController(IStaticAssetsConfiguration staticAssetsConfiguration)
    {
        _staticAssetsConfiguration = staticAssetsConfiguration ?? throw new ArgumentNullException(nameof(staticAssetsConfiguration));
    }

    [HttpGet]
    [Authorize(Roles = "Administrator,Student")]
    public async Task GetPage()
    {
        await HttpContext.Response.RespondWithStaticAsset(_staticAssetsConfiguration.DiskPath, "Student.html");
    }

    [HttpGet("courses")]
    [Authorize(Roles = "Administrator,Student")]
    public async Task GetStudentCourses()
    {
        await HttpContext.Response.RespondWithStaticAsset(_staticAssetsConfiguration.DiskPath, "CoursesAll.html");
    }
}