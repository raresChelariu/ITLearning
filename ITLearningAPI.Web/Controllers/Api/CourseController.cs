using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearningAPI.Web.Authorization;
using ITLearningAPI.Web.Contracts.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly ICourseRepository _courseRepository;

    public CourseController(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
    }

    [Authorize(Policy = AuthorizationPolicies.AdminOrTeacher)]
    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CourseCreateRequest request)
    {
        var user = HttpContext.GetUser();

        var course = new Course
        {
            Name = request.CourseName,
            AuthorId = user.Id
        };
        var courseId = await _courseRepository.Insert(course);
        if (courseId == -1)
        {
            return Conflict();
        }
        return Created($"/api/course?courseId={courseId}", new { courseId });
    }

    [Authorize(Policy = AuthorizationPolicies.AdminOrTeacher)]
    [HttpGet("author")]
    public async Task<IActionResult> GetCoursesForAuthor()
    {
        var user = HttpContext.GetUser();

        var courses = await _courseRepository.GetByAuthorId(user.Id);

        if (courses == null)
            return BadRequest();

        return Ok(courses);
    }

    [Authorize(Policy = AuthorizationPolicies.User)]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _courseRepository.GetAll();

        if (courses == null)
            return BadRequest();

        return Ok(courses);
    }

    [Authorize(Policy = AuthorizationPolicies.AdminOrTeacher)]
    [HttpGet("author/withscripts")]
    public async Task<IActionResult> GetCoursesForAuthorHavingSqlScripts()
    {
        var user = HttpContext.GetUser();

        var courses = await _courseRepository.GetSqlCoursesByUserId(user.Id);

        if (courses is null)
        {
            return BadRequest();
        }
        
        return Ok(courses);
    }
    
    [Authorize(Policy = AuthorizationPolicies.User)]
    [HttpPost("progress/reset")]
    public async Task<IActionResult> ResetCourseProgress(ResetCourseProgressRequest request)
    {
        var user = HttpContext.GetUser();

        await _courseRepository.ResetUserProgress(user.Id, request.CourseId);

        return Ok();
    }
}

