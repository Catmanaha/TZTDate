using TZTDate.Core.Data.DateUser;
using TZTDate.Core.Data.DateUser.Enums;

namespace TZTDate.Core.Data.SearchData;

public class SearchData
{
    public User? Me { get; set; }
    public IEnumerable<User>? Users { get; set; }
    public Gender? SearchingGender { get; set; }
    public string? SearchingUsername { get; set; }
    public int? SearchingStartAge { get; set; }
    public int? SearchingEndAge { get; set; }
    public string? SearchingInterests { get; set; }
}