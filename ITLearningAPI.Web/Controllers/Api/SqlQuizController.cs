using ITLearning.Course.Core.Contracts;
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
    private readonly ISqlQuizAnswerValidator _sqlQuizAnswerValidator;
    
    public SqlQuizController(ISqlQuizRepository sqlQuizRepository, ISqlQuizAnswerValidator sqlQuizAnswerValidator)
    {
        _sqlQuizRepository = sqlQuizRepository ?? throw new ArgumentNullException(nameof(sqlQuizRepository));
        _sqlQuizAnswerValidator = sqlQuizAnswerValidator ?? throw new ArgumentNullException(nameof(sqlQuizAnswerValidator));
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

    [Authorize(Policy = AuthorizationPolicies.User)]
    [HttpPost("validate")]
    public async Task<IActionResult> Validate(SqlQuizValidationRequest request)
    {
        var user = HttpContext.GetUser();
        
        var result = await _sqlQuizAnswerValidator.Validate(new SqlQuizValidationCommand
        {
           UserId = user.Id,
           SqlQuizId = request.SqlQuizId,
           QueryText = request.Query
        });
        
        return Ok(result);
    }
}