using System.ComponentModel.DataAnnotations;
using TZTDate.Core.Data.DateUser.Enums;

namespace TZTBank.Core.Data.DateUser.Dtos;

public class UserRegisterDto
{
    [EmailAddress]
    [Required(ErrorMessage = "Email cannot be empty")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Username cannot be empty")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password cannot be empty")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Age cannot be empty")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Gender cannot be empty")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Location cannot be empty")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Description cannot be empty")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "ProfilePicPath cannot be empty")]
    public string? ProfilePicPath { get; set; }

    public Gender? SearchingGender { get; set; }
    public int SearchingAgeStart { get; set; }
    public int SearchingAgeEnd { get; set; }
    public string? Interests { get; set; }
}
