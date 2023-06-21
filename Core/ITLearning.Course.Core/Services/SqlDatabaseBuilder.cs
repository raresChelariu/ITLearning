using ITLearning.Course.Core.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.Logging;

namespace ITLearning.Course.Core.Services;

internal class SqlDatabaseBuilder : ISqlDatabaseBuilder
{
    private readonly ILogger<SqlDatabaseBuilder> _logger;
    private readonly ISqlPlaygroundRepository _sqlPlaygroundRepository;
    private readonly ICourseScriptRepository _scriptsRepository;

    public SqlDatabaseBuilder(
        ILogger<SqlDatabaseBuilder> logger,
        ISqlPlaygroundRepository sqlPlaygroundRepository,
        ICourseScriptRepository scriptsRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _sqlPlaygroundRepository =
            sqlPlaygroundRepository ?? throw new ArgumentNullException(nameof(sqlPlaygroundRepository));
        _scriptsRepository = scriptsRepository ?? throw new ArgumentNullException(nameof(scriptsRepository));
    }

    public async Task<List<ScriptRunningError>> CreateDatabaseWithScripts(DatabaseBuildCommand command)
    {
        var dbName = GetDatabaseName(command.CourseId, command.UserId);

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
        }
        await _sqlPlaygroundRepository.MarkCourseDatabaseToUser(command.UserId, command.CourseId, dbName);
        _logger.LogInformation("Finished creating {@Database} with scripts", dbName);
        return scriptRunningErrors;
    }

    public async Task RecreateDatabase(long userId, long courseId)
    {
        var scripts = await _scriptsRepository.GetScriptsByCourseId(courseId);
        var command = new DatabaseBuildCommand
        {
            UserId = userId,
            CourseId = courseId,
            SeedingScripts = scripts.ToList()
        };
        _ = await CreateDatabaseWithScripts(command);
    }

    private static string GetDatabaseName(long courseId, long userId)
    {
        const string dbNameTemplate = "Storage_{0}_{1}";
        return string.Format(dbNameTemplate, courseId, userId);
    }
}