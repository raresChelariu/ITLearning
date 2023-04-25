using System.Data;
using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearning.TypeGuards;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ITLearning.Infrastructure.DataAccess.Mssql;

internal class SqlServerCourseScriptRepository : ICourseScriptRepository
{
    private readonly ILogger<SqlServerCourseScriptRepository> _logger; 
    private readonly IDatabaseConnector _databaseConnector;

    public SqlServerCourseScriptRepository(ILogger<SqlServerCourseScriptRepository> logger, IDatabaseConnector databaseConnector)
    {
        _logger = TypeGuard.ThrowIfNull(logger);
        _databaseConnector = TypeGuard.ThrowIfNull(databaseConnector);
    }

    public async Task<int> CreateScript(CourseScript courseScript)
    {
        const string query = "CourseScriptInsert";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
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
            var scriptId = -1;
            if (command.Parameters["@Id"].Value != DBNull.Value)
            {
                scriptId = (int) command.Parameters["@Id"].Value;
            }
            courseScript.ScriptId = scriptId;
            return scriptId;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

    }
}