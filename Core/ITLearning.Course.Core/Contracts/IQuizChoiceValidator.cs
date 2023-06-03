namespace ITLearning.Course.Core.Contracts;

public interface IQuizChoiceValidator
{
    Task<bool> Validate(long quizId, List<long> quizChoiceIds);
}