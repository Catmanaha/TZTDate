using Microsoft.AspNetCore.Identity;
using TZTDate.Core.Data.DateUser.Enums;

namespace TZTDate.Core.Data.DateUser;

public class User : IdentityUser
{
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public string? ProfilePicPath { get; set; }
    public DateTime CreatedAt { get; set; }

    public Gender? SearchingGender { get; set; }
    public int SearchingAgeStart { get; set; }
    public int SearchingAgeEnd { get; set; }
    public string? Interests { get; set; }
}