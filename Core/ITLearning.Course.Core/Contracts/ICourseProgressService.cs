namespace ITLearning.Course.Core.Contracts;

public interface ICourseProgressService
{
    public Task<CourseProgressResult> AdvanceUserToNextItem(long userId, long courseId, long itemId);
}