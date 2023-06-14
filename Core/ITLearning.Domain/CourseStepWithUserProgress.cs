namespace ITLearning.Domain;

public class CourseStepWithUserProgress
{
    public long ItemId { get; set; }
    public string ItemTitle { get; set; }
    public ItemType Type { get; set; }
    public UserProgressForCourseStep Progress { get; set; }
}