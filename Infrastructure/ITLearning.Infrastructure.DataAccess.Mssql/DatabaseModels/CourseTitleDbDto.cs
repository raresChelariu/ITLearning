namespace ITLearning.Infrastructure.DataAccess.Mssql.DatabaseModels;

public class CourseTitleDbDto
{
    public long ItemId { get; set; }
    public string ItemTitle { get; set; }
    public byte ItemTypeId { get; set; }
}