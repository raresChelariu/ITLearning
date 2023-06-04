using ITLearning.Course.Core.Contracts;
using ITLearning.Domain.Models;
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

    public ItemController(
        ICourseItemFetcher itemFetcher,
        ICourseItemRepository itemRepository,
        ICourseProgressService courseProgressService
    )
    {
        _itemFetcher = itemFetcher ?? throw new ArgumentNullException(nameof(itemFetcher));
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        _courseProgressService =
            courseProgressService ?? throw new ArgumentNullException(nameof(courseProgressService));
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
        var itemDetails = await _itemRepository.GetItemDetailsByCourseId(courseId);
        return Ok(itemDetails?.Select(x => new { x.ItemId, x.ItemTitle, Type = x.ItemType.ToString() }));
    }

    [Authorize(Policy = AuthorizationPolicies.User)]
    [HttpPost("next")]
    public async Task<IActionResult> GetNextItem([FromBody] AdvanceToNextItemRequest request)
    {
        var user = HttpContext.GetUser();
        var result = await _courseProgressService.AdvanceUserToNextItem(user.Id, request.CourseId, request.ItemId);
        
        return result.CourseItem.Type switch
        {
            ItemType.Quiz => Ok(new { result.EndOfCourse, courseItem = result.CourseItem as Quiz }),
            ItemType.Wiki => Ok(new { result.EndOfCourse, courseItem = result.CourseItem as CourseWiki }),
            _ => NoContent()
        };
    }
}