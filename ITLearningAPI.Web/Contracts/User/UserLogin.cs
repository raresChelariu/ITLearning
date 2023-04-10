using System.ComponentModel.DataAnnotations;

namespace ITLearningAPI.Web.Contracts.User;

public class UserLogin
{
    [Required]
    public string UserIdentifier { get; set; }

    [Required]
    public string Password { get; set; }
}