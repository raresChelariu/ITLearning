using System.Security.Claims;
using ITLearning.Domain;

namespace ITLearningAPI.Web;

public static class CurrentUser
{
    public static User GetUser(this HttpContext context)
    {
        if (context.User.Identity is not ClaimsIdentity identity)
        {
            return null;
        }

        var userClaims = identity.Claims.ToDictionary(x => x.Type, x => x.Value);
        if (!long.TryParse(userClaims[ClaimTypes.NameIdentifier], out var userId))
        {
            return null;
        }

        return new User
        {
            Username = userClaims[ClaimTypes.Name],
            Role = Enum.Parse<UserRole>(userClaims[ClaimTypes.Role], ignoreCase: true),
            Id = userId,
            Email = userClaims[ClaimTypes.Email]
        };
    }
}