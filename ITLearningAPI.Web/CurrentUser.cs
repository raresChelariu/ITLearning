using System.Security.Claims;
using ITLearning.Domain.Models;

namespace ITLearningAPI.Web;

public static class CurrentUser
{
    public static User GetFromHttpContext(HttpContext context)
    {
        if (context.User.Identity is not ClaimsIdentity identity)
        {
            return null;
        }

        var userClaims = identity.Claims;

        return new User
        {
            Username = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
        };
    }
}