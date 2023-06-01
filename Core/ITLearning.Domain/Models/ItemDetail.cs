namespace ITLearning.Domain.Models;

public class ItemDetail
{
    public long ItemId { get; set; }
    public string ItemTitle { get; set; }
    public ItemType ItemType { get; set; }
}