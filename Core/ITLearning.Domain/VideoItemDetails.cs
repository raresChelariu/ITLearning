namespace ITLearning.Domain;

public class VideoItemDetails : ICourseItem
{
    public ItemType Type => ItemType.Video;
    
    public long ItemId { get; set; }
    public string Title { get; set; }
    public byte[] Content { get; set; }
    public string ContentType { get; set; }
    public long CourseId { get; set; }
}