using System.Security.Claims;
using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearning.Services;
using ITLearning.Utils.Contracts;
using ITLearningAPI.Web.Contracts.User;
using ITLearningAPI.Web.Mappers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public UserController(IUserRepository userRepository, 
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody] UserRegister request)
    {
        var user = request.ToUser();

        var userId = await _userRepository.InsertUserAsync(user);
        if (userId == -1)
        {
            return Conflict();
        }
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] UserLogin request)
    {
        var user = await _userRepository.GetUserByUserIdentifierAsync(request.UserIdentifier);

        if (user == null || !Hasher.VerifyPasswordHash(request.Password, user.PasswordSalt, user.PasswordHash))
        {
            return Unauthorized();
        }

        var jwtToken = CreateToken(user);

        await AddAuthCookie(user, HttpContext);

        return Ok(new
        {
            Token = jwtToken,
            Role = user.Role.ToString().ToLowerInvariant(),
        });
    }

    private async Task AddAuthCookie(User user, HttpContext context)
    {
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Email));
        identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
        identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
        var principal = new ClaimsPrincipal(identity);
        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = _dateTimeProvider.UtcNow.AddDays(1)
            });
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Response.Cookies.Delete(".AspNetCore.Session");
        HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "Administrator,Teacher")]
    public IActionResult SomeEndpoint()
    {
        var user = HttpContext.GetUser();
        return Ok(new
        {
            user.Username,
            user.Email,
            user.Role
        });
    }

    private static string CreateToken(User user)
    {
        var userForRead = user.ToUserForRead();
        var serializedUser = userForRead.JsonSerialized();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, serializedUser),
            new(ClaimTypes.Role, userForRead.Role.ToString())
        };
        
        

        return null;
    }

}