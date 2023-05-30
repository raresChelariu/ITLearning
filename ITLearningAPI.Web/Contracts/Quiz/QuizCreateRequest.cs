namespace ITLearningAPI.Web.Contracts.Quiz;

public class QuizCreateRequest
{
    public long CourseId { get; set; }
    public string QuestionText { get; set; }

    public IEnumerable<QuizChoiceTextAndCorrectnessRequestDto> Choices { get; set; }
    public string QuizTitle { get; set; }
}