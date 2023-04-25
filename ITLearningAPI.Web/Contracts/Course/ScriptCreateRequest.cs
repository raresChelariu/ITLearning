using System.ComponentModel.DataAnnotations;

namespace ITLearningAPI.Web.Contracts.Course;

public class ScriptCreateRequest
{
    [Required]
    public string SeedingScript { get; set; }

    [Required]
    public string DatabaseSystem { get; set; }

    [Required]
    public long CourseId { get; set; }

    [Required]
    public string ScriptName { get; set; }
}