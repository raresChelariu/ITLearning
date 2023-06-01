using ITLearning.Domain.Models;

namespace ITLearning.Infrastructure.DataAccess.Contracts;

public interface IQuizRepository
{
    Task<long> CreateQuiz(Quiz quiz);
    Task<Quiz> GetByItemId(long itemId);
}