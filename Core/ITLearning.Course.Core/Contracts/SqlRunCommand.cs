namespace ITLearning.Course.Core.Contracts;

public class SqlRunCommand
{
    public long UserId { get; set; }
    public long CourseId { get; set; }
    public string Query { get; set; }
}