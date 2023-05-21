using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Contracts;
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

    [Authorize(Policy = "AdminOrTeacher")]
    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CourseCreateRequest request)
    {
        var user = HttpContext.GetUser();

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

    [Authorize(Policy = "AdminOrTeacher")]
    [HttpGet]
    public async Task<IActionResult> GetCoursesForAuthor()
    {
        var user = HttpContext.GetUser();

        var courses = await _courseRepository.GetByAuthorId(user.Id);

        if (courses == null)
            return BadRequest();

        return Ok(courses);
    }

    [HttpGet("/all")]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _courseRepository.GetAll();

        if (courses == null)
            return BadRequest();

        return Ok(courses);
    }
}

