using System.ComponentModel.DataAnnotations;

namespace ITLearningAPI.Web.Contracts.Course;

public class CourseCreateRequest
{
    [Required]
    public string CourseName;
}