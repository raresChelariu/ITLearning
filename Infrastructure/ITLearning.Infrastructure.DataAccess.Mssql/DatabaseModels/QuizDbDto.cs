namespace ITLearning.Infrastructure.DataAccess.Mssql.DatabaseModels;

internal class QuizDbDto
{
    public long CourseId { get; set; }
    public long ItemId { get; set; }
    public string QuestionText { get; set; }
}