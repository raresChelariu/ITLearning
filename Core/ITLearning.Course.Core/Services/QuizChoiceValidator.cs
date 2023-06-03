using ITLearning.Course.Core.Contracts;
using ITLearning.Infrastructure.DataAccess.Contracts;

namespace ITLearning.Course.Core.Services;

internal class QuizChoiceValidator : IQuizChoiceValidator
{
    private readonly IQuizRepository _quizRepository;

    public QuizChoiceValidator(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository ?? throw new ArgumentNullException(nameof(quizRepository));
    }

    public async Task<bool> Validate(long quizId, List<long> quizChoiceIds)
    {
        var quiz = await _quizRepository.GetByItemId(quizId);
        if (quiz is null)
        {
            return false;
        }
        var correctChoiceIds = quiz.PossibleAnswers
            .Where(x => x.CorrectChoice)
            .Select(x => x.QuizChoiceId)
            .ToList();
        
        return correctChoiceIds.Count == quizChoiceIds.Count && 
               correctChoiceIds.All(quizChoiceIds.Contains);
    }
}