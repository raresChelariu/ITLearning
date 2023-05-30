namespace ITLearningAPI.Web.Contracts.CourseWiki;

public class CourseWikiCreateRequest
{
    public long CourseId { get; set; }
    public string CourseWikiText { get; set; }
    public string Title { get; set; }
}