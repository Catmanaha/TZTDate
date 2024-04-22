using TZTDate.Core.Data.DateUser.Enums;

namespace TZTDate.Core.Data.DateUser;

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public DateTime BirthDateTime { get; set; }

    public int Age
    {
        get
        {
            var today = int.Parse(DateTime.Today.ToString("yyyMMdd"));
            var age = today - int.Parse(BirthDateTime.ToString("yyyMMdd"));
            return age / 10000;
        }
    }

    public Gender Gender { get; set; }
    public Address? Address { get; set; }
    public string? Description { get; set; }
    public string[]? ProfilePicPaths { get; set; }
    public DateTime CreatedAt { get; set; }

    public Gender? SearchingGender { get; set; }
    public int SearchingAgeStart { get; set; }
    public int SearchingAgeEnd { get; set; }
    public string? Interests { get; set; }

    public List<UserFollow>? Followers { get; set; }
    public List<UserFollow>? Followed { get; set; }
}