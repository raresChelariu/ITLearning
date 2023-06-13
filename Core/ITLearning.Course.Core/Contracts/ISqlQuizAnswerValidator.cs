namespace ITLearning.Course.Core.Contracts;

public interface ISqlQuizAnswerValidator
{
    Task<SqlQuizValidationResult> Validate(SqlQuizValidationCommand command);
}