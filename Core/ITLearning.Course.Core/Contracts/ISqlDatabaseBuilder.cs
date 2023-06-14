using ITLearning.Infrastructure.DataAccess.Contracts;

namespace ITLearning.Course.Core.Contracts;

public interface ISqlDatabaseBuilder
{
    Task<List<ScriptRunningError>> CreateDatabaseWithScripts(DatabaseBuildCommand command);
    Task RecreateDatabase(long userId, long courseId);
}