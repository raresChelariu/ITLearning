namespace ITLearning.Course.Core.Contracts;

public class SqlQuizValidationResult
{
    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; }
}