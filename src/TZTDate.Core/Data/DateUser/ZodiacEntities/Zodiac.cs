using Newtonsoft.Json;
using TZTDate.Core.Data.DateUser.Enums;

namespace TZTDate.Core.Data.DateUser.ZodiacEntities;

public class RelationshipData
{
    public string? Type { get; set; }
    public string? Description { get; set; }
}

public static class ZodiacSign
{
    public static Zodiac? GetZodiacSign(this User user)
    {
        int day = user.BirthDateTime.Day;
        int month = user.BirthDateTime.Month;
        switch (month)
        {
            case 1:
                return day <= 19 ? Zodiac.Capricorn : Zodiac.Aquarius;
            case 2:
                return day <= 18 ? Zodiac.Aquarius : Zodiac.Pisces;
            case 3:
                return day <= 21 ? Zodiac.Pisces : Zodiac.Aries;
            case 4:
                return day <= 19 ? Zodiac.Aries : Zodiac.Taurus;
            case 5:
                return day <= 20 ? Zodiac.Taurus : Zodiac.Gemini;
            case 6:
                return day <= 20 ? Zodiac.Gemini : Zodiac.Cancer;
            case 7:
                return day <= 22 ? Zodiac.Cancer : Zodiac.Leo;
            case 8:
                return day <= 22 ? Zodiac.Leo : Zodiac.Virgo;
            case 9:
                return day <= 22 ? Zodiac.Virgo : Zodiac.Libra;
            case 10:
                return day <= 22 ? Zodiac.Libra : Zodiac.Scorpio;
            case 11:
                return day <= 21 ? Zodiac.Scorpio : Zodiac.Sagittarius;
            case 12:
                return day <= 21 ? Zodiac.Sagittarius : Zodiac.Capricorn;
            default:
                return null;
        }
    }

    public static RelationshipData? CompatibilityByZodiacSign(this User currentUser, User userForCompare)
    {
        string jsonFilePath = "../TZTDate.Core/Data/DateUser/ZodiacEntities/CompabilityZodiac.json";
        string json = File.ReadAllText(jsonFilePath);
        var relationshipList = JsonConvert.DeserializeObject<List<RelationshipData>>(json);
        var zodiacSignDistance = currentUser.GetZodiacSign() - userForCompare.GetZodiacSign();
        switch (zodiacSignDistance)
        {
            case 0:
                return relationshipList?[0];
            case 1:
            case -1:
            case 11:
            case -11:
                return relationshipList?[1];
            case 2:
            case -2:
            case 10:
            case -10:
                return relationshipList?[2];
            case 3:
            case -3:
            case 9:
            case -9:
                return relationshipList?[3];
            case 4:
            case -4:
            case 8:
            case -8:
                return relationshipList?[4];
            case 5:
            case -5:
            case 7:
            case -7:
                return relationshipList?[5];
            case 6:
            case -6:
                return relationshipList?[6];
            default: 
                return null;
        }
    }
}