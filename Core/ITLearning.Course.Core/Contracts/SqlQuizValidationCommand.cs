namespace ITLearning.Course.Core.Contracts;

public class SqlQuizValidationCommand
{
    public long UserId { get; set; }
    public long SqlQuizId { get; set; }
    public string QueryText { get; set; }
}