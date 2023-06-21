using ITLearning.Course.Core.Contracts;
using ITLearning.Domain;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.Logging;

namespace ITLearning.Course.Core.Services;

public class CourseStepProgressService : ICourseStepProgressService
{
    private readonly ILogger<CourseStepProgressService> _logger;
    private readonly ICourseItemRepository _itemRepository;
    private readonly ICourseRepository _courseRepository;

    public CourseStepProgressService(
        ILogger<CourseStepProgressService> logger,
        ICourseItemRepository itemRepository,
        ICourseRepository courseRepository)
    {
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<CourseStepWithUserProgress>> GetStepTitlesWithProgress(long userId, long courseId)
    {
        var itemDetails = await _itemRepository.GetItemDetailsByCourseId(courseId);
        if (itemDetails is null)
        {
            _logger.LogError("No item details found for {@CourseId}", courseId);
            return null;
        }

        var result = itemDetails.Select(x => new CourseStepWithUserProgress
        {
            ItemId = x.ItemId,
            ItemTitle = x.ItemTitle,
            Type = x.ItemType,
            Progress = UserProgressForCourseStep.NotStarted
        }).ToList();

        var lastItemId = await _courseRepository.GetUsersLastItemId(userId, courseId);
        if (lastItemId == -1)
        {
            result[0].Progress = UserProgressForCourseStep.InProgress;
            return result;
        }

        var currentUserItemIndex = 0L;
        for (var index = 0; index < result.Count; index++)
        {
            var item = result[index];
            if (item.ItemId == lastItemId)
            {
                currentUserItemIndex = index + 1;
                break;
            }
        }

        if (currentUserItemIndex == result.Count)
        {
            foreach (var item in result)
            {
                item.Progress = UserProgressForCourseStep.Completed;
            }

            return result;
        }

        result[(int)currentUserItemIndex].Progress = UserProgressForCourseStep.InProgress;
        for (var index = 0; index < currentUserItemIndex; index++)
        {
            var item = result[index];
            item.Progress = UserProgressForCourseStep.Completed;
        }

        return result;
    }
    
    
}