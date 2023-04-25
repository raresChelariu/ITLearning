using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearning.TypeGuards;
using ITLearningAPI.Web.Contracts.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase
{
    private readonly ICourseRepository _courseRepository;

    public CourseController(ICourseRepository courseRepository)
    {
        _courseRepository = TypeGuard.ThrowIfNull(courseRepository);
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> CreateCourse([FromBody] CourseCreateRequest request)
    {
        var user = CurrentUser.GetFromHttpContext(HttpContext);
        
        var course = new Course
        {
            Name = request.CourseName,
            AuthorId = user.Id
        };

        if (await _courseRepository.Insert(course) == -1)
        {
            return Conflict();
        }
        return CreatedAtAction(nameof(CreateCourse), request);
    }

    [HttpGet]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> GetCoursesForAuthor()
    {
        var user = CurrentUser.GetFromHttpContext(HttpContext);

        var courses = await _courseRepository.GetByAuthorId(user.Id);

        if (courses == null)
            return BadRequest();

        return Ok(courses);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _courseRepository.GetAll();

        if (courses == null)
            return BadRequest();

        return Ok(courses);
    }
}

