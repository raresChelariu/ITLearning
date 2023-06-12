namespace ITLearningAPI.Web.Contracts.SqlPlayground;

public class RunQueryRequest
{
    public long CourseId { get; set; }
    public string QueryText { get; set; }
}