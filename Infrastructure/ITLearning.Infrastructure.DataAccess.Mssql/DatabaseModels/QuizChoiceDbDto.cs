namespace ITLearning.Infrastructure.DataAccess.Mssql.DatabaseModels;

internal class QuizChoiceDbDto
{
    public string ChoiceText { get; set; }
    public long QuizChoiceId { get; set; }
    public bool CorrectChoice { get; set; }
}