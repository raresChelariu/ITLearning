using ITLearning.Course.Core.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.Logging;

namespace ITLearning.Course.Core.Services;

internal class SqlDatabaseBuilder : ISqlDatabaseBuilder
{
    private readonly ILogger<SqlDatabaseBuilder> _logger;
    private readonly ISqlPlaygroundRepository _sqlPlaygroundRepository;
    public SqlDatabaseBuilder(ILogger<SqlDatabaseBuilder> logger, 
        ISqlPlaygroundRepository sqlPlaygroundRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _sqlPlaygroundRepository = sqlPlaygroundRepository ?? throw new ArgumentNullException(nameof(sqlPlaygroundRepository));
    }

    public async Task<List<ScriptRunningError>> CreateDatabaseWithScripts(DatabaseBuildCommand command)
    {
        var dbName = GetDatabaseName(command);

        var scriptRunningErrors = new List<ScriptRunningError>();
        
        _logger.LogInformation("(Re)Creating Course {@Database}", dbName);
        await _sqlPlaygroundRepository.DropDatabaseIfExists(dbName);
        await _sqlPlaygroundRepository.CreateDatabase(dbName);

        foreach (var script in command.SeedingScripts)
        {
            _logger.LogInformation("Running on {@Database} the script {@ScriptId}", dbName, script.ScriptId);
            var scriptRunningError = await _sqlPlaygroundRepository.RunScriptForDatabase(dbName, script.ScriptText);
            if (scriptRunningError is not null)
            {
                scriptRunningErrors.Add(scriptRunningError);
            }
            await _sqlPlaygroundRepository.MarkCourseDatabaseToUser(command.UserId, command.CourseId, dbName);
        }
        _logger.LogInformation("Finished creating {@Database} with scripts", dbName);
        return scriptRunningErrors;
    }

    private static string GetDatabaseName(DatabaseBuildCommand command)
    {
        const string dbNameTemplate = "Storage_{0}_{1}";
        return string.Format(dbNameTemplate, command.CourseId, command.UserId);
    }
}