using ITLearning.Domain.Models;
using ITLearningAPI.Web.Contracts.User;
using ITLearning.Services;

namespace ITLearningAPI.Web.Mappers;

public static class Mapper
{
    public static async Task<Video> ToVideoAsync(IFormFile file)
    {
        var fileName = file.FileName;
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();
        var video = new Video
        {
            Content = fileBytes,
            Name = fileName
        };
        return video;
    }

    public static User ToUser(UserRegister request)
    {
        Hasher.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = (UserRole) request.Role
        };
        return user;
    }

}