using System.Data;
using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ITLearning.Infrastructure.DataAccess.Mssql;

internal class SqlServerCourseScriptRepository : ICourseScriptRepository
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
        await using var connection = _databaseConnector.GetSqlConnection();
        try
        {
            await connection.OpenAsync();
            var command = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = query,
                Connection = connection
            };
            command.Parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.Output,
                ParameterName = "@Id",
                SqlDbType = SqlDbType.Int
            });

            await command.ExecuteNonQueryAsync();
            var scriptId = -1L;
            if (command.Parameters["@Id"].Value != DBNull.Value)
            {
                scriptId = (int)command.Parameters["@Id"].Value;
            }

            courseScript.ScriptId = scriptId;
            return scriptId;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(CreateScript), ex);
            return -1;
        }
        finally
        { 
            await connection.DisposeAsync();
        }
    }

    public async Task<CourseScript> GetScriptsByScriptId(long scriptId)
    {
        const string query = "SELECT ScriptID, CourseID, DatabaseSystem, ScriptName, SeedingScript FROM CourseScripts WHERE [ScriptID] = @ScriptId";
        await using var connection = _databaseConnector.GetSqlConnection();
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ScriptId", scriptId);
            var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var courseScript = CreateScriptFromReader(reader);
                return courseScript;
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(GetScriptsByScriptId), ex);
            return null;
        }
    }

    private static CourseScript CreateScriptFromReader(SqlDataReader reader)
    {
        return new CourseScript
        {
            CourseId = reader.GetFromColumn<long>("CourseID"),
            DatabaseSystem = reader.GetFromColumn<string>("DatabaseSystem"),
            ScriptId = reader.GetFromColumn<long>("ScriptID"),
            ScriptName = reader.GetFromColumn<string>("ScriptName"),
            SeedingScript = reader.GetFromColumn<string>("SeedingScript")
        };
    }
}