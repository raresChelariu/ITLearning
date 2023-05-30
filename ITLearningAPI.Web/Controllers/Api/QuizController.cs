using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearningAPI.Web.Authorization;
using ITLearningAPI.Web.Contracts;
using ITLearningAPI.Web.Contracts.Quiz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class QuizController : ControllerBase
{
    private readonly IQuizRepository _quizRepository;
    private readonly ICourseRepository _courseRepository;

    public QuizController(IQuizRepository quizRepository, ICourseRepository courseRepository)
    {
        _quizRepository = quizRepository ?? throw new ArgumentNullException(nameof(quizRepository));
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
    }

    [Authorize(Policy =  AuthorizationPolicies.Teacher)]
    [HttpPost]
    public async Task<IActionResult> CreateQuiz([FromBody] QuizCreateRequest request)
    {
        var user = HttpContext.GetUser();

        var teacherId = user.Id;
        var teacherCourses = await _courseRepository.GetByAuthorId(teacherId);
        if (teacherCourses.All(x => x.Id != request.CourseId))
        {
            return BadRequest(new ApiError
            {
                ErrorMessage = "Course ID does not belong to logged teacher"
            });
        }

        var quiz = new Quiz
        {
            CourseId = request.CourseId,
            QuestionText = request.QuestionText,
            QuizTitle = request.QuizTitle,
            PossibleAnswers = request.Choices.Select(x => new QuizChoice
            {
                ChoiceText = x.ChoiceText,
                CorrectChoice = x.IsRight
            }).ToList()
        };
        var result = await _quizRepository.CreateQuiz(quiz);
        if (result == -1)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(CreateQuiz), quiz);
    }
}