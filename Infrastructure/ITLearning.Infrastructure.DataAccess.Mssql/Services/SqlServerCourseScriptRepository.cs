using System.Data;
using Dapper;
using ITLearning.Domain;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.Logging;

namespace ITLearning.Infrastructure.DataAccess.Mssql.Services;

public class SqlServerCourseScriptRepository : ICourseScriptRepository
{
    private readonly ILogger<SqlServerCourseScriptRepository> _logger;
    private readonly IDatabaseConnector _databaseConnector;
    
    public SqlServerCourseScriptRepository(ILogger<SqlServerCourseScriptRepository> logger, IDatabaseConnector databaseConnector)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
    }
    
    public async Task<long> CreateScript(CourseScript courseScript)
    {
        const string query = "CourseScriptInsert";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                CourseID = courseScript.CourseId,
                courseScript.ScriptText
            });
            var scriptId = await connection.ExecuteScalarAsync<long>(query, parameters, null, null, CommandType.StoredProcedure);
            if (scriptId == default)
            {
                _logger.LogWarning("{@Operation} returned {@ScriptId} with default value!", nameof(CreateScript), courseScript.ScriptId);
            }
            courseScript.ScriptId = scriptId;
            return scriptId;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(CreateScript), ex);
            return -1;
        }
    }

    public async Task<IEnumerable<CourseScript>> GetScriptsByCourseId(long courseId)
    {
        const string query = "SELECT CourseID, ScriptID, ScriptText FROM CourseScripts where CourseID = @CourseID";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                CourseID = courseId
            });
            var result = await connection.QueryAsync<CourseScript>(query, parameters);
            if (result is null)
            {
                _logger.LogWarning("{@Operation} returned null!", nameof(GetScriptsByCourseId));
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetScriptsByCourseId), ex);
            return null;
        }
    }
}