using TZTDate.Core.Data.DateUser;

namespace TZTDate.Core.Data.DateUserAndRecomendations;

public class DateUserAndRecomendations {
    public User? Me { get; set; }
    public IEnumerable<User>? RecomendationUsers { get; set; }
}