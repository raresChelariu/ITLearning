using ITLearning.Course.Core.Contracts;
using ITLearning.Domain;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearningAPI.Web.Authorization;
using ITLearningAPI.Web.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly ICourseItemFetcher _itemFetcher;
    private readonly ICourseItemRepository _itemRepository;
    private readonly ICourseProgressService _courseProgressService;
    private readonly ICourseStepProgressService _courseStepProgressService;
    
    public ItemController(
        ICourseItemFetcher itemFetcher,
        ICourseItemRepository itemRepository,
        ICourseProgressService courseProgressService,
        ICourseStepProgressService courseStepProgressService)
    {
        _itemFetcher = itemFetcher ?? throw new ArgumentNullException(nameof(itemFetcher));
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        _courseProgressService =
            courseProgressService ?? throw new ArgumentNullException(nameof(courseProgressService));
        _courseStepProgressService = courseStepProgressService ?? throw new ArgumentNullException(nameof(courseStepProgressService));
    }

    [Authorize(Policy = AuthorizationPolicies.User)]
    [HttpGet]
    public async Task<IActionResult> GetItemById([FromQuery] long itemId)
    {
        var item = await _itemFetcher.GetByItemId(itemId);
        return Ok(item);
    }

    [Authorize(Policy = AuthorizationPolicies.User)]
    [HttpGet("course/{courseId:long}")]
    public async Task<IActionResult> GetItemDetailsByCourseId([FromRoute] long courseId)
    {
        var user = HttpContext.GetUser();
        var result = await _courseStepProgressService.GetStepTitlesWithProgress(user.Id, courseId);
        
        return Ok(result?.Select(x => new
        {
            x.ItemId,
            x.ItemTitle,
            Type = x.Type.ToString(),
            Progress = (byte) x.Progress
        }));
    }

    [Authorize(Policy = AuthorizationPolicies.User)]
    [HttpPost("next")]
    public async Task<IActionResult> GetNextItem([FromBody] AdvanceToNextItemRequest request)
    {
        var user = HttpContext.GetUser();
        var result = await _courseProgressService.AdvanceUserToNextItem(user.Id, request.CourseId, request.ItemId);
        if (result == null)
        {
            return NoContent();
        }
        if (result.EndOfCourse)
        {
            return Ok(result);
        }
        
        return result.CourseItem.Type switch
        {
            ItemType.Quiz => Ok(new { result.EndOfCourse, courseItem = result.CourseItem as Quiz }),
            ItemType.Wiki => Ok(new { result.EndOfCourse, courseItem = result.CourseItem as CourseWiki }),
            ItemType.Video => Ok(new { result.EndOfCourse, courseItem = result.CourseItem as VideoItemDetails }),
            ItemType.SqlQuiz => Ok(new { result.EndOfCourse, courseItem = result.CourseItem as SqlQuiz }),
            _ => NoContent()
        };
    }
}