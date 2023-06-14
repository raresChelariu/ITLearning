using ITLearning.Domain;

namespace ITLearning.Infrastructure.DataAccess.Contracts;

public interface IUserRepository
{
    Task<long> InsertUserAsync(User user);
    Task<User> GetUserByUserIdentifierAsync(string username);
    Task<User> GetUserByEmailAsync(string email);
}