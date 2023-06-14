using ITLearning.Domain;

namespace ITLearning.Course.Core.Contracts;

public class DatabaseBuildCommand
{
    public long UserId { get; set; }
    public long CourseId { get; set; }
    public List<CourseScript> SeedingScripts { get; set; }
}