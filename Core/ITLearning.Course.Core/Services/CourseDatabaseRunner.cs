using System.Data;
using ITLearning.Course.Core.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.Logging;

namespace ITLearning.Course.Core.Services;

internal class CourseDatabaseRunner : ICourseDatabaseRunner
{
    private readonly ILogger<CourseDatabaseRunner> _logger;
    private readonly ISqlPlaygroundRepository _sqlPlaygroundRepository;
    
    public CourseDatabaseRunner(ILogger<CourseDatabaseRunner> logger
        , ISqlPlaygroundRepository sqlPlaygroundRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _sqlPlaygroundRepository = sqlPlaygroundRepository ?? throw new ArgumentNullException(nameof(sqlPlaygroundRepository));
    }
    
    public async Task<DataTable> GetQueryResult(SqlRunCommand command)
    {
        var databaseName = await _sqlPlaygroundRepository.GetCourseDatabaseName(command.UserId, command.CourseId);
        if (databaseName is null)
        {
            _logger.LogWarning("Database name is null for {@UserId} and {@CourseId}", command.UserId, command.CourseId);
            return null;
        }
        var dataTable = await _sqlPlaygroundRepository.GetQueryResults(databaseName, command.Query);
        if (dataTable is null)
        {
            _logger.LogWarning("DataTable is null for {@UserId}, {@CourseId}, {@UserSqlQuery}", 
                command.UserId, command.CourseId, command.Query);
            return null;
        }

        return dataTable;
    }
}