namespace TZTDate.Core.Data.DateUser;

using Microsoft.AspNetCore.Identity;
using TZTDate.Core.Data.DateUser.Enums;

public class User : IdentityUser
{
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
    public Zodiac? ZodiacSign
    {
        get
        {
            int day = BirthDateTime.Day;
            int month = BirthDateTime.Month;

            switch (month)
            {
                case 1:
                    return (day <= 19) ? Zodiac.Capricorn : Zodiac.Aquarius;
                case 2:
                    return (day <= 18) ? Zodiac.Aquarius : Zodiac.Pisces;
                case 3:
                    return (day >= 21) ? Zodiac.Aries : Zodiac.Pisces;
                case 4:
                    return (day <= 19) ? Zodiac.Aries : Zodiac.Taurus;
                case 5:
                    return (day <= 20) ? Zodiac.Taurus : Zodiac.Gemini;
                case 6:
                    return (day <= 20) ? Zodiac.Gemini : Zodiac.Cancer;
                case 7:
                    return (day <= 22) ? Zodiac.Cancer : Zodiac.Leo;
                case 8:
                    return (day <= 22) ? Zodiac.Leo : Zodiac.Virgo;
                case 9:
                    return (day <= 22) ? Zodiac.Virgo : Zodiac.Libra;
                case 10:
                    return (day <= 22) ? Zodiac.Libra : Zodiac.Scorpio;
                case 11:
                    return (day <= 21) ? Zodiac.Scorpio : Zodiac.Sagittarius;
                case 12:
                    return (day <= 21) ? Zodiac.Sagittarius : Zodiac.Capricorn;
                default:
                    return Zodiac.Capricorn;
            }
        }
    }

    public Gender? SearchingGender { get; set; }
    public int SearchingAgeStart { get; set; }
    public int SearchingAgeEnd { get; set; }
    public string? Interests { get; set; }

    //public IEnumerable<User>? LikedPersons { get; set; }
}