namespace ITLearning.Infrastructure.DataAccess.Mssql.DatabaseModels;

internal class ItemDetailsDbDto
{
    public long ItemId { get; set; }
    public string ItemTitle { get; set; }
    public byte ItemTypeId { get; set; }
}