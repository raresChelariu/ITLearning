using ITLearning.Domain;

namespace ITLearning.Course.Core.Interfaces;

internal interface IItemFetcherStrategyCollection
{
    public Func<long, Task<ICourseItem>> GetStrategyByItemType(ItemType itemType);
}