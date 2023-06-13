using System.Data;
using Dapper;
using ITLearning.Domain;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.Logging;

namespace ITLearning.Infrastructure.DataAccess.Mssql.Services;

internal class SqlServerSqlQuizRepository : ISqlQuizRepository
{
    private readonly ILogger<SqlServerSqlQuizRepository> _logger;
    private readonly IDatabaseConnector _databaseConnector;

    public SqlServerSqlQuizRepository(
        ILogger<SqlServerSqlQuizRepository> logger
        , IDatabaseConnector databaseConnector
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
    }

    public async Task<long> CreateSqlQuiz(SqlQuiz quiz)
    {
        const string query = "SqlQuizInsert";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                CourseID = quiz.CourseId,
                quiz.QuestionText,
                QuizTitle = quiz.Title,
                quiz.ExpectedQuery
            });
            var result = await connection.ExecuteScalarAsync<long>(query, parameters,
                null, null, CommandType.StoredProcedure);
            quiz.ItemId = result;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(CreateSqlQuiz), ex);
            return -1;
        }
    }
}