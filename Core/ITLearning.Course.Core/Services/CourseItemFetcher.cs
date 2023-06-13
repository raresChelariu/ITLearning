using ITLearning.Course.Core.Contracts;
using ITLearning.Course.Core.Interfaces;
using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Contracts;

namespace ITLearning.Course.Core.Services;

internal class CourseItemFetcher : ICourseItemFetcher
{
    private readonly ICourseItemRepository _itemRepository;
    private readonly IItemFetcherStrategyCollection _itemFetcherCollection;
    
    public CourseItemFetcher(
        ICourseItemRepository itemRepository,
        IItemFetcherStrategyCollection itemFetcherStrategyCollection)
    {
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        _itemFetcherCollection = itemFetcherStrategyCollection ?? throw new ArgumentNullException(nameof(itemFetcherStrategyCollection));
    }

    public async Task<ICourseItem> GetByItemId(long itemId)
    {
        var itemDetails = await _itemRepository.GetItemDetailById(itemId);
        if (itemDetails is null)
        {
            return null;
        }

        var fetcher = _itemFetcherCollection.GetStrategyByItemType(itemDetails.ItemType);
        var item = await fetcher(itemId);
        if (item is null)
        {
            return null;
        }
        item.Title = itemDetails.ItemTitle;
        return item;
    }
    
}