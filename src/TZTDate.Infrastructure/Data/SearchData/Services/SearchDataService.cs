using TZTDate.Core.Data.DateUser;
using TZTDate.Core.Data.DateUser.Enums;

namespace TZTDate.Infrastructure.Data.SearchData.Services;

public static class SearchDataService
{
    public static List<User> ProfilesFilter(Core.Data.SearchData.SearchData searchData)
    {
        var users = searchData.Users;
        var me = searchData.Me;
        var searchByName = searchData.SearchingUsername;
        var startAge = searchData.SearchingStartAge;
        var endAge = searchData.SearchingEndAge;
        var interests = searchData.SearchingInterests;
        var searchGender = searchData.SearchingGender;

        users = users.Where(u => u.Id != me?.Id).ToList();

        if (searchByName is null && startAge is null && endAge is null && interests is null && searchGender is null)
        {
            users = users.Where(u => u.Age >= me?.SearchingAgeStart).ToList();
            users = users.Where(u => u.Age <= me?.SearchingAgeEnd).ToList();
            users = users.Where(u => u.Gender == me?.SearchingGender).ToList();
        }

        else
        {
            if (!string.IsNullOrEmpty(searchByName))
            {
                users = users.Where(u => u.Username.ToLower().Contains(searchByName.ToLower())).ToList();
            }

            if (startAge.HasValue && startAge != 0)
            {
                users = users.Where(u => u.Age >= startAge).ToList();
            }

            if (endAge.HasValue && endAge != 0)
            {
                users = users.Where(u => u.Age <= endAge).ToList();
            }

            if (searchGender is not null)
            {
                users = users.Where(u => u.Gender == searchGender).ToList();
            }

            if (interests is not null)
            {
                string[] interestsArray = interests.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                users = users.Where(u => u.Interests != null && u.Interests.Split(' ', StringSplitOptions.RemoveEmptyEntries).Intersect(interestsArray).Any()).ToList();
            }
        }

        return users.ToList();
    }

    public static List<User> MoreProfilesFilter(Core.Data.SearchData.SearchData searchData)
    {
        var users = searchData.Users;
        var me = searchData.Me;
        var searchByName = searchData.SearchingUsername;
        var startAge = searchData.SearchingStartAge;
        var endAge = searchData.SearchingEndAge;
        var interests = searchData.SearchingInterests;
        var searchGender = searchData.SearchingGender;

        users = users.Where(u => u.Id != me.Id).ToList();

        if (!string.IsNullOrEmpty(searchByName))
        {
            users = users.Where(u => u.Username.ToLower().Contains(searchByName.ToLower())).ToList();
        }

        if (startAge.HasValue && startAge != 0)
        {
            users = users.Where(u => u.Age >= startAge).ToList();
        }

        if (endAge.HasValue && endAge != 0)
        {
            users = users.Where(u => u.Age <= endAge).ToList();
        }

        if (searchGender is not null)
        {
            users = users.Where(u => u.Gender == searchGender).ToList();
        }

        if (!string.IsNullOrWhiteSpace(interests))
        {
            string[] interestsArray = interests.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            users = users.Where(u => u.Interests != null && u.Interests.Split(' ', StringSplitOptions.RemoveEmptyEntries).Intersect(interestsArray).Any()).ToList();
        }

        return users.ToList();
    }
}