using System.ComponentModel.DataAnnotations;

namespace TZTBank.Core.Data.DateUser.Dtos;

public class UserLoginDto
{

    [EmailAddress]
    [Required(ErrorMessage = "Email cannot be empty")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password cannot be empty")]
    public string? Password { get; set; }
}
