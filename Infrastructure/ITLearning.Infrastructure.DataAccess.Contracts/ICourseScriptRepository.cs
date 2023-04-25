using ITLearning.Domain.Models;

namespace ITLearning.Infrastructure.DataAccess.Contracts;

public interface ICourseScriptRepository
{
    Task<int> CreateScript(CourseScript courseScript);
}