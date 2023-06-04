namespace ITLearningAPI.Web.Contracts;

public class AdvanceToNextItemRequest
{
    public long CourseId { get; set; }
    public long ItemId { get; set; }
}