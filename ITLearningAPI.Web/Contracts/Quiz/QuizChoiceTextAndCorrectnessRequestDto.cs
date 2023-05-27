namespace ITLearningAPI.Web.Contracts.Quiz;

public class QuizChoiceTextAndCorrectnessRequestDto
{
    public string ChoiceText { get; set; }
    public bool IsRight { get; set; }
}