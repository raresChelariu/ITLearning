using System.Data;
using Dapper;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ITLearning.Infrastructure.DataAccess.Mssql.Services;

internal class SqlServerSqlPlaygroundRepository : ISqlPlaygroundRepository
{
    private readonly ILogger<SqlServerSqlPlaygroundRepository> _logger;
    private readonly IDatabaseConnector _databaseConnector;

    public SqlServerSqlPlaygroundRepository(ILogger<SqlServerSqlPlaygroundRepository> logger,
        IDatabaseConnector databaseConnector)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
    }

    public async Task CreateDatabase(string databaseName)
    {
        var query = @"CREATE DATABASE [Xy] CONTAINMENT = NONE ON  PRIMARY 
        ( NAME = N'Xy', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Xy.mdf' , SIZE = 8192KB , FILEGROWTH = 65536KB )
        LOG ON 
        ( NAME = N'Xy_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Xy_log.ldf' , SIZE = 8192KB , FILEGROWTH = 65536KB )
        WITH LEDGER = OFF".Replace("Xy", databaseName);
        
        try
        {
            var connection = _databaseConnector.GetSqlConnectionMasterDatabase();
            await connection.OpenAsync();
            var command = new SqlCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(CreateDatabase), ex);
        }
    }

    public async Task CreateSomeTable(string databaseName)
    {
        const string query = "CREATE TABLE Pineapple (PineappleID BIGINT NOT NULL, DateOfBirth datetime2 null)";

        try
        {
            var connection = _databaseConnector.GetSqlConnectionCustomDatabase(databaseName);
            await connection.OpenAsync();
            var command = new SqlCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(CreateSomeTable), ex);
        }
    }

    public async Task<ScriptRunningError> RunScriptForDatabase(string dbName, string scriptText)
    {
        try
        {
            var connection = _databaseConnector.GetSqlConnectionCustomDatabase(dbName);
            await connection.OpenAsync();
            var command = new SqlCommand(scriptText, connection);
            await command.ExecuteNonQueryAsync();
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception} {@DatabaseName} {@ScriptText}",
                nameof(CreateSomeTable), ex, dbName, scriptText);
            return new ScriptRunningError
            {
                Exception = ex
            };
        }
    }

    public async Task MarkCourseDatabaseToUser(long userId, long courseId, string dbName)
    {
        try
        {
            const string query = "INSERT INTO PlaygroundUsers (CourseID, UserID, DatabaseName) VALUES (@CourseID, @UserID, @DatabaseName)";
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new 
            {
                CourseID = courseId,
                UserID = userId,
                DatabaseName = dbName
            });
            await connection.ExecuteAsync(query, parameters);
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(MarkCourseDatabaseToUser), ex);
        }
    }

    public async Task DropDatabaseIfExists(string databaseName)
    {
        const string queryFormat = @"DECLARE @SQL VARCHAR(MAX);
            SELECT @SQL=COALESCE(@SQL, '')+'Kill '+CONVERT(VARCHAR, SPId)+';'
            FROM master..SysProcesses
            WHERE DBId=DB_ID('{0}')AND SPId<>@@SPId;
            EXEC(@SQL);
            DROP DATABASE IF EXISTS {0};";
        var query = string.Format(queryFormat, databaseName);
        try
        {
            var connection = _databaseConnector.GetSqlConnectionMasterDatabase();
            var command = new SqlCommand(query, connection);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(DropDatabaseIfExists), ex);
        }
    }

    public async Task<string> GetCourseDatabaseName(long userId, long courseId)
    {
        const string query = "SELECT DatabaseName FROM PlaygroundUsers WHERE CourseID = @CourseID AND UserID = @UserID";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new 
            {
                UserID = userId,
                CourseID = courseId
            });
            var result = await connection.ExecuteScalarAsync<string>(query, parameters);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(DropDatabaseIfExists), ex);
            return null;
        }
    }
    
    public async Task<DataTable> GetQueryResults(string dbName, string query)
    {
        try
        {
            var connection = _databaseConnector.GetSqlConnectionCustomDatabase(dbName);
            await connection.OpenAsync();
            var command = new SqlCommand(query, connection);
            var reader = await command.ExecuteReaderAsync();
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(DropDatabaseIfExists), ex);
            return null;
        }
    }
}