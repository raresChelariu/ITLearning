using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Contracts;
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
        _courseScriptRepository = courseScriptRepository ?? throw new ArgumentNullException(nameof(courseScriptRepository));
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
    }

    [Authorize(Policy = "AdminOrTeacher")]
    [HttpPost]
    public async Task<IActionResult> CreateScript([FromBody] ScriptCreateRequest request)
    {
        var user = HttpContext.GetUser();

        var authorsCourse = await _courseRepository.GetByAuthorIdAndCourseId(user.Id, request.CourseId);
        if (authorsCourse == null)
        {
            return Conflict(new ApiError { ErrorMessage = "Course does not match the courses of the teacher" });
        }

        if (!Enum.TryParse(request.DatabaseSystem, ignoreCase: true, out DatabaseSystems databaseSystem))
        {
            return BadRequest(ApiError.WithParameterException(request.DatabaseSystem));
        }

        var courseScript = new CourseScript
        {
            CourseId = request.CourseId,
            SeedingScript = request.SeedingScript,
            DatabaseSystem = databaseSystem,
            ScriptName = request.ScriptName
        };

        await _courseScriptRepository.CreateScript(courseScript);

        return Ok(courseScript);
    }
}