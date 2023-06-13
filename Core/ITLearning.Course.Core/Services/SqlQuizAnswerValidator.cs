using System.Data;
using ITLearning.Course.Core.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ITLearning.Course.Core.Services;

public class SqlQuizAnswerValidator : ISqlQuizAnswerValidator
{
    private readonly ILogger<SqlQuizAnswerValidator> _logger;
    private readonly ISqlQuizRepository _sqlQuizRepository;
    private readonly ICourseDatabaseRunner _databaseRunner;
    
    public SqlQuizAnswerValidator(
        ILogger<SqlQuizAnswerValidator> logger,
        ISqlQuizRepository sqlQuizRepository,
        ICourseDatabaseRunner databaseRunner)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _sqlQuizRepository = sqlQuizRepository ?? throw new ArgumentNullException(nameof(sqlQuizRepository));
        _databaseRunner = databaseRunner ?? throw new ArgumentNullException(nameof(databaseRunner));
    }

    public async Task<SqlQuizValidationResult> Validate(SqlQuizValidationCommand command)
    {
        var quiz = await _sqlQuizRepository.GetSqlQuizById(command.SqlQuizId);
        if (quiz is null)
        {
            _logger.LogWarning("No SQL Quiz found to validate for {@SqlQuizId}", command.SqlQuizId);
            return new SqlQuizValidationResult
            {
                IsValid = false,
                ErrorMessage = "SQL Quiz-ul nu exista"
            };
        }
        var userResult = await _databaseRunner.GetQueryResult(new SqlRunCommand
        {
            CourseId = quiz.CourseId,
            UserId = command.UserId,
            Query = command.QueryText
        });
        if (userResult is null)
        {
            return new SqlQuizValidationResult
            {
                IsValid = false,
                ErrorMessage = "Interogarea utilizatorului nu a putut fi rulata cu succes"
            };
        }
        var expectedResult = await _databaseRunner.GetQueryResult(new SqlRunCommand
        {
            CourseId = quiz.CourseId,
            UserId = command.UserId,
            Query = quiz.ExpectedQuery 
        });
        if (expectedResult is null)
        {
            return new SqlQuizValidationResult
            {
                IsValid = false,
                ErrorMessage = "Interogarea raspunsului corect nu a putut fi rulata cu succes"
            };
        }
        if (userResult.Columns.Count != expectedResult.Columns.Count)
        {
            const string messageDifferentColumnCount =
                "Interogarea a returnat {0} coloane, dar trebuia sa returneze {1} coloane";
            return new SqlQuizValidationResult
            {
                IsValid = false,
                ErrorMessage = string.Format(messageDifferentColumnCount, userResult.Columns.Count, expectedResult.Columns.Count)
            };
        }
        var userColumnNames = GetColumnNames(userResult);
        var expectedColumnNames = GetColumnNames(expectedResult);

        userColumnNames.Sort();
        expectedColumnNames.Sort();

        for (var index = 0; index < userColumnNames.Count; index++)
        {
            if (userColumnNames[index] != expectedColumnNames[index])
            {
                return new SqlQuizValidationResult
                {
                    IsValid = false,
                    ErrorMessage =
                        $"Coloana se numeste {userColumnNames[index]}, dar trebuia sa se numeasca {expectedColumnNames[index]}"
                };
            }
        }

        if (userResult.Rows.Count != expectedResult.Rows.Count)
        {
            const string messageDifferentRowCount =
                "Interogarea a returnat {0} randuri, dar trebuia sa returneze {1} randuri";
            return new SqlQuizValidationResult
            {
                IsValid = false,
                ErrorMessage = string.Format(messageDifferentRowCount, userResult.Rows.Count, expectedResult.Rows.Count)
            };
        }

        var userResultJson = DataTableParsedToJson(userResult);
        var expectedResultJson = DataTableParsedToJson(expectedResult);
        if (userResultJson != expectedResultJson)
        {
            return new SqlQuizValidationResult
            {
                IsValid = false,
                ErrorMessage = "Coloanele si numarul de randuri sunt corecte, dar continutul randurilor difera"
            };   
        }
        return new SqlQuizValidationResult
        {
            IsValid = true,
            ErrorMessage = ""
        };
    }

    private static string DataTableParsedToJson(DataTable dt)
    {
        return JsonConvert.SerializeObject(dt);
    }
    
    private static List<string> GetColumnNames(DataTable dt)
    {
        return dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
    }
}