namespace ITLearningAPI.Web.Contracts.SqlQuiz;

public class SqlQuizCreateRequest
{
    public long CourseId { get; set; }
    public string Title { get; set; }
    public string QuestionText { get; set; }
    public string ExpectedQuery { get; set; }
}