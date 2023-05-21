using System.Security.Claims;

namespace ITLearningAPI.Web.Authorization;

public interface IJwtService
{
    string CreateJwtToken(ICollection<Claim> claims);

    ICollection<Claim> GetClaims(string token);
}