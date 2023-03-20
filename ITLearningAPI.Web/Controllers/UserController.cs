﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Contracts;
using ITLearning.TypeGuards;
using ITLearningAPI.Web.Authorization;
using ITLearningAPI.Web.Contracts.User;
using ITLearningAPI.Web.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ITLearningAPI.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IAuthorizationSettings _authorizationSettings;
    private readonly IUserRepository _userRepository;

    public UserController(IAuthorizationSettings authorizationSettings, IUserRepository userRepository)
    {
        _authorizationSettings = TypeGuard.ThrowIfNull(authorizationSettings);
        _userRepository = TypeGuard.ThrowIfNull(userRepository);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody] UserRegister request)
    {
        var user = Mapper.ToUser(request);

        var userId = await _userRepository.InsertUserAsync(user);
        if (userId == -1)
        {
            return Conflict();
        }
        return Ok(user);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] UserLogin request)
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Username);
        
        if (user == null)
        {
            return BadRequest();
        }

        var jwtToken = CreateToken(user, DateTime.Now.AddDays(1));

        return Ok(jwtToken);
    }

    [HttpGet]
    [Authorize(Roles="Administrator,Teacher")]
    public IActionResult SomeEndpoint()
    {
        var user = CurrentUser.GetFromHttpContext(HttpContext);
        return Ok(new
        {
            user.Username,
            user.Email,
            user.Role
        });
    }

    private string CreateToken(User user, DateTime expirationDateTime)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(user.Username));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(_authorizationSettings.Issuer, _authorizationSettings.Audience, claims: claims, expires: expirationDateTime, signingCredentials: credentials);
        
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

}