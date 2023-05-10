using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearning.TypeGuards;
using ITLearningAPI.Web.Contracts;
using ITLearningAPI.Web.Contracts.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class CourseScriptController : ControllerBase
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICourseScriptRepository _courseScriptRepository;

    public CourseScriptController(ICourseScriptRepository courseScriptRepository, ICourseRepository courseRepository)
    {
        _courseScriptRepository = TypeGuard.ThrowIfNull(courseScriptRepository);
        _courseRepository = TypeGuard.ThrowIfNull(courseRepository);
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> CreateScript([FromBody] ScriptCreateRequest request)
    {
        var user = CurrentUser.GetUser(HttpContext);

        var authorsCourse = await _courseRepository.GetByAuthorIdAndCourseId(user.Id, request.CourseId);
        if (authorsCourse == null)
        {
            return Conflict(new ApiError { ErrorMessage = "Course does not match the courses of the teacher" });
        }

        var courseScript = new CourseScript
        {
            CourseId = request.CourseId,
            SeedingScript = request.SeedingScript,
            DatabaseSystem = request.DatabaseSystem,
            ScriptName = request.ScriptName
        };

        await _courseScriptRepository.CreateScript(courseScript);

        return Ok(courseScript);
    }
}