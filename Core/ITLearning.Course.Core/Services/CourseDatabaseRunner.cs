using ITLearning.Course.Core.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
    
    public async Task<string> GetQueryResult(long userId, long courseId, string query)
    {
        var databaseName = await _sqlPlaygroundRepository.GetCourseDatabaseName(userId, courseId);
        if (databaseName is null)
        {
            _logger.LogWarning("Database name is null for {@UserId} and {@CourseId}", userId, courseId);
            return null;
        }
        var dataTable = await _sqlPlaygroundRepository.GetQueryResults(databaseName, query);
        if (dataTable is null)
        {
            _logger.LogWarning("DataTable is null for {@UserId}, {@CourseId}, {@UserSqlQuery}", userId, courseId, query);
            return null;
        } 
        var result = JsonConvert.SerializeObject(dataTable);
        
        return result;
    }
}