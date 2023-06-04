using ITLearning.Domain.Models;

namespace ITLearning.Course.Core.Contracts;

public class CourseProgressResult
{
    public ICourseItem CourseItem { get; set; }
    public bool EndOfCourse { get; set; }
}