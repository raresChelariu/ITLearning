using ITLearning.Course.Core.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearningAPI.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly ICourseItemFetcher _itemFetcher;
    private readonly ICourseItemRepository _itemRepository;

    public ItemController(ICourseItemFetcher itemFetcher, ICourseItemRepository itemRepository)
    {
        _itemFetcher = itemFetcher ?? throw new ArgumentNullException(nameof(itemFetcher));
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
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
        return Ok(itemDetails?.Select(x => new { x.ItemId, x.ItemTitle, Type = x.ItemType.ToString()}));
    }
}