using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearningAPI.Web.Contracts;
using ITLearningAPI.Web.Contracts.CourseWiki;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class CourseWikiController : ControllerBase
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICourseWikiRepository _courseWikiRepository;
    
    public CourseWikiController(ICourseRepository courseRepository, ICourseWikiRepository courseWikiRepository)
    {
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        _courseWikiRepository = courseWikiRepository ?? throw new ArgumentNullException(nameof(courseWikiRepository));
    }

    [Authorize(Policy = "AdminOrTeacher")]
    [HttpPost]
    public async Task<IActionResult> CreateCourseWiki([FromBody] CourseWikiCreateRequest request)
    {
        var user = HttpContext.GetUser();
        if (user.Role == UserRole.Teacher)
        {
            var course = await _courseRepository.GetByAuthorIdAndCourseId(user.Id, request.CourseId);
            if (course is null)
            {
                return BadRequest(new ApiError
                {
                    ErrorMessage = "Course does not exist for author"
                });
            }
        }
        var wiki = new CourseWiki
        {
            CourseId = request.CourseId,
            CourseWikiText = request.CourseWikiText,
            Title = request.Title
        };
        var result = await _courseWikiRepository.CreateWiki(wiki);
        if (result == -1)
        {
            return BadRequest(new ApiError
            {
                ErrorMessage = "Failed to create wiki" 
            });
        }
        return CreatedAtAction(nameof(CreateCourseWiki), wiki);
    }

    [Authorize(Policy = "User")]
    [HttpGet]
    public async Task<IActionResult> GetWikiByItemId([FromQuery] long itemId)
    {
        var wiki = await _courseWikiRepository.GetWikiByItemId(itemId);
        if (wiki is null)
        {
            return NotFound();
        }
        return Ok(wiki);
    }
}