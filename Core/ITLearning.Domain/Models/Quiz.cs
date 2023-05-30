namespace ITLearning.Domain.Models;

public class Quiz
{
    public long CourseId { get; set; }
    public long ItemId { get; set; }
    public string QuestionText { get; set; }
    
    public List<QuizChoice> PossibleAnswers { get; set; }
    public string QuizTitle { get; set; }
}