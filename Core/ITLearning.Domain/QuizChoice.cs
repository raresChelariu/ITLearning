namespace ITLearning.Domain.Models;

public class QuizChoice
{
    public long QuizChoiceId { get; set; }
    public long QuizId { get; set; }
    public string ChoiceText { get; set; }
    public bool CorrectChoice { get; set; }
}