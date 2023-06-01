using System.Data;
using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Common.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.Logging;
using Dapper;
using ITLearning.Infrastructure.DataAccess.Mssql.DatabaseModelMapping;
using ITLearning.Infrastructure.DataAccess.Mssql.DatabaseModels;

namespace ITLearning.Infrastructure.DataAccess.Mssql;

public class SqlServerQuizRepository : IQuizRepository
{
    private readonly ILogger<SqlServerUserRepository> _logger;
    private readonly IDatabaseConnector _databaseConnector;

    public SqlServerQuizRepository(ILogger<SqlServerUserRepository> logger, IDatabaseConnector databaseConnector)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _databaseConnector = databaseConnector ?? throw new ArgumentNullException(nameof(databaseConnector));
    }

    public async Task<long> CreateQuiz(Quiz quiz)
    {
        const string query = "QuizInsert";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var choices = quiz.PossibleAnswers.Select(x => new QuizChoiceTextAndCorrectness
            {
                QuizChoiceText = x.ChoiceText,
                IsRight = x.CorrectChoice
            }).ToList();
            var choicesDataTable = CreateChoicesDataTable(choices);

            var parameters = new DynamicParameters(new
            {
                CourseID = quiz.CourseId,
                quiz.QuestionText,
                QuizTitle = quiz.Title,
                PossibleAnswers = choicesDataTable.AsTableValuedParameter("QuizChoicesList")
            });
            var quizResult = await connection.QueryFirstOrDefaultAsync<long>(query, parameters, commandType: CommandType.StoredProcedure);
            quiz.ItemId = quizResult;

            return quizResult;
        }
        catch (Exception ex)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(CreateQuiz), ex);
            return -1;
        }
    }

    public async Task<Quiz> GetByItemId(long itemId)
    {
        const string query = "QuizWithAnswersByItemId";
        try
        {
            var connection = _databaseConnector.GetSqlConnection();
            var parameters = new DynamicParameters(new
            {
                ItemID = itemId
            });
            var dbResultSets = await connection.QueryMultipleAsync(query, parameters, null, null, CommandType.StoredProcedure);
            var quiz = dbResultSets.Read<QuizDbDto>().FirstOrDefault();
            var quizChoices = dbResultSets.Read<QuizChoiceDbDto>();
            var result = QuizMapper.FromQuizDbAndQuizChoiceDb(quiz, quizChoices);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Db failure for {@Operation}! {@Exception}", nameof(CreateQuiz), e);
            return null;
        }
    }

    private static DataTable CreateChoicesDataTable(IEnumerable<QuizChoiceTextAndCorrectness> choices)
    {
        var table = new DataTable();
        table.Columns.Add("ChoiceText", typeof(string));
        table.Columns.Add("IsCorrect", typeof(bool));

        foreach (var choice in choices)
        {
            table.Rows.Add(choice.QuizChoiceText, choice.IsRight);
        }
        
        return table;
    }
}