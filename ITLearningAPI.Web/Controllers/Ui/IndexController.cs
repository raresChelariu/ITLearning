using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Ui;

[ApiController]
public class IndexController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    [Route("/")]
    public IActionResult GetDefaultPage()
    {
        if (HttpContext.User.Identity is null || !HttpContext.User.Identity.IsAuthenticated)
        {
            return Redirect("/login");
        }

        var claims = HttpContext.User.Claims;
        var roleClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
        var roleValue = roleClaim?.Value;
        if (string.IsNullOrEmpty(roleValue))
        {
            return Redirect("/login");
        }

        roleValue = roleValue.ToLowerInvariant();
        return roleValue switch
        {
            "teacher" => Redirect("/teacher"),
            "student" => Redirect("/student"),
            _ => Redirect("/login")
        };
    }
}