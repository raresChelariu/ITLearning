﻿using ITLearning.Domain.Models;

namespace ITLearning.Infrastructure.DataAccess.Contracts;

public interface IUserRepository
{
    Task<long> InsertUserAsync(User user);
    Task<User> GetUserByUsernameAsync(string username);
    Task<User> GetUserByEmailAsync(string email);
}