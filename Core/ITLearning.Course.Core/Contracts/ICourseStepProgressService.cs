using ITLearning.Domain;

namespace ITLearning.Course.Core.Contracts;

public interface ICourseStepProgressService
{
    public Task<IEnumerable<CourseStepWithUserProgress>> GetStepTitlesWithProgress(long userId, long courseId);
}