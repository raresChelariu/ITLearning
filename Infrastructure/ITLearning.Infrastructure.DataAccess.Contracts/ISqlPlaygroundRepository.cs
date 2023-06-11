using System.Data;

namespace ITLearning.Infrastructure.DataAccess.Contracts;

public interface ISqlPlaygroundRepository
{
    Task CreateDatabase(string databaseName);

    Task<ScriptRunningError> RunScriptForDatabase(string dbName, string scriptText);
    Task MarkCourseDatabaseToUser(long userId, long courseId, string dbName);
    Task DropDatabaseIfExists(string databaseName);
    Task<string> GetCourseDatabaseName(long userId, long courseId);

    Task<DataTable> GetQueryResults(string dbName, string query);
}