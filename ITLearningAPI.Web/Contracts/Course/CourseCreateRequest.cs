using System.ComponentModel.DataAnnotations;

namespace ITLearningAPI.Web.Contracts.Course;

public class CourseCreateRequest
{
    [Required]
    public string CourseName { get; set; }
    
    [Required]
    public string CourseDescription { get; set; }
}