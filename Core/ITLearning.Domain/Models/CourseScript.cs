namespace ITLearning.Domain.Models;

public class CourseScript
{
    public string SeedingScript { get; set; }
    public DatabaseSystems DatabaseSystem { get; set; }
    public long CourseId { get; set; }

    public string ScriptName { get; set; }
    public long ScriptId { get; set; }
}