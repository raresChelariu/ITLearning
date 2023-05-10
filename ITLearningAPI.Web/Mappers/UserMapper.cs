using System.Text.Json;
using ITLearning.Domain.Models;
using ITLearningAPI.Web.Contracts.User;
using ITLearning.Services;

namespace ITLearningAPI.Web.Mappers;

public static class UserMapper
{
    public static User ToUser(this UserRegister request)
    {
        Hasher.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = (UserRole)request.Role
        };
        return user;
    }

    public static UserForRead ToUserForRead(this User user)
    {
        return new UserForRead
        {
            Username = user.Username,
            Email = user.Email,
            Id = user.Id,
            Role = user.Role
        };
    }

    public static string JsonSerialized(this UserForRead userForRead)
    {
        return JsonSerializer.Serialize(userForRead);
    }
}