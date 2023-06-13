namespace ITLearning.Domain.Models;

public class Course
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long AuthorId { get; set; }
    public DateTime CreatedDateTime { get; set; }
}