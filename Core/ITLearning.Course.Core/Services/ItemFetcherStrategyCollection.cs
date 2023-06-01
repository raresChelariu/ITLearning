using ITLearning.Course.Core.Interfaces;
using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.Logging;

namespace ITLearning.Course.Core.Services;

internal class ItemFetcherStrategyCollection : IItemFetcherStrategyCollection
{
    private readonly ILogger<ItemFetcherStrategyCollection> _logger;
    private readonly ICourseWikiRepository _courseWikiRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly Dictionary<ItemType, Func<long, Task<ICourseItem>>> _itemTypeToStrategy;

    public ItemFetcherStrategyCollection(
        ILogger<ItemFetcherStrategyCollection> logger,
        ICourseWikiRepository courseWikiRepository,
        IQuizRepository quizRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _itemTypeToStrategy = GetMappings();
        _courseWikiRepository = courseWikiRepository ?? throw new ArgumentNullException(nameof(courseWikiRepository));
        _quizRepository = quizRepository ?? throw new ArgumentNullException(nameof(quizRepository));
    }

    public Func<long, Task<ICourseItem>> GetStrategyByItemType(ItemType itemType)
    {
        var isStrategyDefined = _itemTypeToStrategy.TryGetValue(itemType, out var strategy);
        if (!isStrategyDefined)
        {
            _logger.LogError("No database fetcher defined for {@ItemType}", itemType);
            return null;
        }

        return strategy;
    }

    private Dictionary<ItemType, Func<long, Task<ICourseItem>>> GetMappings()
    {
        return new Dictionary<ItemType, Func<long, Task<ICourseItem>>>
        {
            {
                ItemType.Quiz,
                async itemId => await _quizRepository.GetByItemId(itemId)
            },
            {
                ItemType.Wiki,
                async itemId => await _courseWikiRepository.GetWikiByItemId(itemId)
            }
        };
    }
}