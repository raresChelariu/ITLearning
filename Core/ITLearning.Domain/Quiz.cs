namespace ITLearning.Domain;

public class Quiz : ICourseItem
{
    public ItemType Type => ItemType.Quiz;
    public long CourseId { get; set; }
    public long ItemId { get; set; }
    public string QuestionText { get; set; }
    
    public List<QuizChoice> PossibleAnswers { get; set; }
    public string Title { get; set; }
}