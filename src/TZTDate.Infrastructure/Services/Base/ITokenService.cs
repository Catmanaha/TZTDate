using TZTDate.Core.Data.DateUser;

namespace TZTDate.Infrastructure.Services.Base;

public interface ITokenService
{
    public string CreateToken(User user);
    public string CreateTokenAdmin(User user);
}
