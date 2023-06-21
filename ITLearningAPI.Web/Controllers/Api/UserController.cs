using System.Security.Claims;
using ITLearning.Domain;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearning.Services;
using ITLearning.Utils.Contracts;
using ITLearningAPI.Web.Authorization;
using ITLearningAPI.Web.Contracts.User;
using ITLearningAPI.Web.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITLearningAPI.Web.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IJwtService _jwtService;

    public UserController(IUserRepository userRepository
        , IDateTimeProvider dateTimeProvider
        , IJwtService jwtService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
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

        if (user is null || !Hasher.VerifyPasswordHash(request.Password, user.PasswordSalt, user.PasswordHash))
        {
            return Unauthorized();
        }

        var jwtToken = CreateToken(user);

        AddAuthCookie(jwtToken, HttpContext);

        return Ok(new
        {
            Token = jwtToken,
            Role = user.Role.ToString().ToLowerInvariant(),
        });
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("logout")]
    public Task<IActionResult> Logout()
    {
        HttpContext.Response.Cookies.Delete("AuthToken");
        return Task.FromResult<IActionResult>(Ok());
    }

    [Authorize(Policy = "Administrator,Teacher")]
    [HttpGet]
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

    private string CreateToken(User user)
    {
        var userForRead = user.ToUserForRead();
        var uniquenessId = Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userForRead.Username),
            new(ClaimTypes.Role, userForRead.Role.ToString()),
            new(ClaimTypes.NameIdentifier, userForRead.Id.ToString()),
            new(ClaimTypes.Email, userForRead.Email),
            new(ClaimTypes.GivenName, uniquenessId)
        };
        var token = _jwtService.CreateJwtToken(claims);
        return token;
    }

    private void AddAuthCookie(string cookieValue, HttpContext context)
    {
        context.Response.Cookies.Append("AuthToken", cookieValue, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = _dateTimeProvider.UtcNow.AddMinutes(60),
            SameSite = SameSiteMode.Strict
        });
    }
}