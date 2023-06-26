namespace ITLearning.Domain;

public class Video : ICourseItem
{
    public ItemType Type => ItemType.Quiz;
    
    public long ItemId { get; set; }
    public string Title { get; set; }
    public byte[] Content { get; set; }
    public string ContentType { get; set; }
    public long CourseId { get; set; }
    
    public string Filename { get; set; }
}