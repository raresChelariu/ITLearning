using ITLearning.Domain;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearningAPI.Web.Authorization;
using ITLearningAPI.Web.Contracts.SqlQuiz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class SqlQuizController : ControllerBase
{
    private readonly ISqlQuizRepository _sqlQuizRepository;
    
    public SqlQuizController(ISqlQuizRepository sqlQuizRepository)
    {
        _sqlQuizRepository = sqlQuizRepository ?? throw new ArgumentNullException(nameof(sqlQuizRepository));
    }

    [Authorize(Policy = AuthorizationPolicies.AdminOrTeacher)]
    [HttpPost]
    public async Task<IActionResult> CreateSqlQuiz(SqlQuizCreateRequest request)
    {
        var sqlQuiz = new SqlQuiz
        {
            CourseId = request.CourseId,
            ExpectedQuery = request.ExpectedQuery,
            QuestionText = request.QuestionText,
            Title = request.Title
        };
        var result = await _sqlQuizRepository.CreateSqlQuiz(sqlQuiz);
        return Created("/api/sqlquiz", result);
    }
    
}