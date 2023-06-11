namespace ITLearning.Course.Core.Contracts;

public interface ICourseDatabaseRunner
{
    Task<string> GetQueryResult(long userId, long courseId, string query);
}