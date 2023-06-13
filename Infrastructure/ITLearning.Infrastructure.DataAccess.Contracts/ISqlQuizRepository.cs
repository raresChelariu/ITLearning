using ITLearning.Domain;

namespace ITLearning.Infrastructure.DataAccess.Contracts;

public interface ISqlQuizRepository
{
    Task<long> CreateSqlQuiz(SqlQuiz quiz);
}