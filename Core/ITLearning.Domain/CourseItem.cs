namespace ITLearning.Domain.Models;

public interface ICourseItem
{
    long ItemId { get; }
    ItemType Type { get; }
    string Title { get; set; }
}