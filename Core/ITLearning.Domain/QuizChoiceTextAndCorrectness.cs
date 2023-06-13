namespace ITLearning.Domain.Models;

public record QuizChoiceTextAndCorrectness
{
    public string QuizChoiceText { get; set; }
    public bool IsRight { get; set; }
}