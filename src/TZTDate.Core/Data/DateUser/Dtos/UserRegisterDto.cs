using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
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

    [Required(ErrorMessage = "BirthDateTime cannot be empty")]
    public DateTime BirthDateTime { get; set; }

    [Required(ErrorMessage = "Gender cannot be empty")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Country cannot be empty")]
    public string? Country { get; set; }

    [Required(ErrorMessage = "City cannot be empty")]
    public string? City { get; set; }

    [Required(ErrorMessage = "State cannot be empty")]
    public string? State { get; set; }

    [Required(ErrorMessage = "Description cannot be empty")]
    public string? Description { get; set; }

    [Required]
    public Gender? SearchingGender { get; set; }

    [Required]
    [Range(int.MinValue, double.MaxValue, ErrorMessage = "SearchingAgeStart cannot be negative or be more than 100")]
    public int SearchingAgeStart { get; set; }
    
    [Required]
    [Range(int.MinValue, 100, ErrorMessage = "SearchingAgeEnd cannot be negative or be more than 100")]
    public int SearchingAgeEnd { get; set; }

    [Required(ErrorMessage = "Interests cannot be empty")]
    public string? Interests { get; set; }

    public IFormFile? Image1 { get; set; }
    public IFormFile? Image2 { get; set; }
    public IFormFile? Image3 { get; set; }
    public IFormFile? Image4 { get; set; }
    public IFormFile? Image5 { get; set; }
    public IFormFile? Image6 { get; set; }
}
