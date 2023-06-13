namespace ITLearning.Domain.Models;

public class CourseWiki : ICourseItem
{
    public ItemType Type => ItemType.Wiki;
    public long CourseId { get; set; }
    public long ItemId { get; set; }
    public string CourseWikiText { get; set; }
    public string Title { get; set; }
}