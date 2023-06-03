namespace ITLearningAPI.Web.Contracts.Quiz;

public class QuizChoiceValidationRequest
{
    public long QuizId { get; set; }
    public List<long> QuizChoiceIds { get; set; } 
}