using TZTDate.Core.Data.DateUser;

namespace TZTDate.Infrastructure.Data.SearchData.Services;

public static class SearchDataService
{
    public static List<User> ProfilesFilter(Core.Data.SearchData.SearchData searchData)
    {
        var users = searchData.Users.AsEnumerable();
        var me = searchData.Me;

        if (me == null)
        {
            throw new ArgumentNullException(nameof(me));
        }

        users = users.Where(u => u.Id != me.Id);

        if (searchData.SearchingUsername is null && searchData.SearchingStartAge is null && searchData.SearchingEndAge is null && searchData.SearchingInterests is null && searchData.SearchingGender is null)
        {
            users = users.Where(u => u.Age >= me.SearchingAgeStart && u.Age <= me.SearchingAgeEnd && u.Gender == me.SearchingGender);
        }
        else
        {
            if (!string.IsNullOrEmpty(searchData.SearchingUsername))
            {
                users = users.Where(u => u.Username.Contains(searchData.SearchingUsername));
            }

            if (searchData.SearchingStartAge.HasValue && searchData.SearchingStartAge != 0)
            {
                users = users.Where(u => u.Age >= searchData.SearchingStartAge);
            }

            if (searchData.SearchingEndAge.HasValue && searchData.SearchingEndAge != 0)
            {
                users = users.Where(u => u.Age <= searchData.SearchingEndAge);
            }

            if (searchData.SearchingGender is not null)
            {
                users = users.Where(u => u.Gender == searchData.SearchingGender);
            }

            if (searchData.SearchingInterests is not null)
            {
                string[] interestsArray = searchData.SearchingInterests.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                users = users.Where(u => u.Interests != null && u.Interests.Split(' ', StringSplitOptions.RemoveEmptyEntries).Intersect(interestsArray).Any());
            }
        }

        return users.ToList();
    }

    public static IQueryable<User> MoreProfilesFilter(Core.Data.SearchData.SearchData searchData)
    {
        var users = searchData.Users;
        var me = searchData.Me;

        if (me == null)
        {
            throw new ArgumentNullException(nameof(me));
        }

        users = users.Where(u => u.Id != me.Id).AsQueryable();

        if (!string.IsNullOrEmpty(searchData.SearchingUsername))
        {
            users = users.Where(u => u.Username.Equals(searchData.SearchingUsername, StringComparison.OrdinalIgnoreCase)).AsQueryable();
        }

        if (searchData.SearchingStartAge.HasValue && searchData.SearchingStartAge != 0)
        {
            users = users.Where(u => u.Age >= searchData.SearchingStartAge).AsQueryable();
        }

        if (searchData.SearchingEndAge.HasValue && searchData.SearchingEndAge != 0)
        {
            users = users.Where(u => u.Age <= searchData.SearchingEndAge).AsQueryable();
        }

        if (searchData.SearchingGender is not null)
        {
            users = users.Where(u => u.Gender == searchData.SearchingGender).AsQueryable();
        }

        if (!string.IsNullOrWhiteSpace(searchData.SearchingInterests))
        {
            string[] interestsArray = searchData.SearchingInterests.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            users = users.Where(u => u.Interests != null && u.Interests.Split(' ', StringSplitOptions.RemoveEmptyEntries).Intersect(interestsArray).Any()).AsQueryable();
        }

        return users.AsQueryable();
    }
}