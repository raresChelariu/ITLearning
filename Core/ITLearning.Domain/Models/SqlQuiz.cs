using ITLearning.Domain.Models;

namespace ITLearning.Domain;

public class SqlQuiz : ICourseItem
{
    public ItemType Type => ItemType.SqlQuiz;
    
    public long ItemId { get; set; }
    
    public string Title { get; set; }
    
    public long CourseId { get; set; }
    
    public string QuestionText { get; set; }
    public string ExpectedQuery { get; set; }
}