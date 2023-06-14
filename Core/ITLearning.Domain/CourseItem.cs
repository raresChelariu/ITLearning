namespace ITLearning.Domain;

public interface ICourseItem
{
    long ItemId { get; }
    ItemType Type { get; }
    string Title { get; set; }
}