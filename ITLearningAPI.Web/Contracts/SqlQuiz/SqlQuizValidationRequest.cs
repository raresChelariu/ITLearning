namespace ITLearningAPI.Web.Contracts.SqlQuiz;

public class SqlQuizValidationRequest
{
    public long SqlQuizId { get; set; }
    public string Query { get; set; }
}