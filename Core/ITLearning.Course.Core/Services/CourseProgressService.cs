using ITLearning.Course.Core.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;

namespace ITLearning.Course.Core.Services;

public class CourseProgressService : ICourseProgressService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICourseItemFetcher _itemFetcher;

    public CourseProgressService(ICourseRepository courseRepository, ICourseItemFetcher itemFetcher)
    {
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        _itemFetcher = itemFetcher ?? throw new ArgumentNullException(nameof(itemFetcher));
    }

    public async Task<CourseProgressResult> AdvanceUserToNextItem(long userId, long courseId, long itemId)
    {
        await _courseRepository.UpdateUserCourseProgress(userId, courseId, itemId);
        var result = await _courseRepository.GetNextItemId(courseId, itemId);
        if (result is null)
        {
            return null;
        }
        if (result.IsEndOfCourse)
        {
            return new CourseProgressResult
            {
                EndOfCourse = true,
                CourseItem = null
            };
        }
        var item = await _itemFetcher.GetByItemId(result.NextItemId);
        return new CourseProgressResult
        {
            EndOfCourse = false,
            CourseItem = item
        };
    }
}