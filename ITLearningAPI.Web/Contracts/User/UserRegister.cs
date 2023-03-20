using System.ComponentModel.DataAnnotations;

namespace ITLearningAPI.Web.Contracts.User;

public class UserRegister
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public short Role { get; set; }
}