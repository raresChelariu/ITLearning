using System.Security.Claims;
using ITLearning.Domain.Models;

namespace ITLearningAPI.Web;

public static class CurrentUser
{
    public static User GetUser(this HttpContext context)
    {
        if (context.User.Identity is not ClaimsIdentity identity)
        {
            return null;
        }

        var userClaims = identity.Claims.ToDictionary(x => x.Type, x => x);
        
        return new User
        {
            Username = userClaims[ClaimTypes.NameIdentifier].Value,
            Email = userClaims[ClaimTypes.Email].Value,
            Role = Enum.Parse<UserRole>(userClaims[ClaimTypes.Role].Value, ignoreCase: true)
        };
    }
}