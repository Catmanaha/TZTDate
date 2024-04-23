using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TZTDate.Core.Data.DateUser;

namespace TZTDate.Infrastructure.Services.Base;

public interface ITokenService
{
    public string CreateToken(IEnumerable<Claim> claims);
    public Task<bool> ValidateToken(string accessToken);
    public JwtSecurityToken ReadToken(string accessToken);
    public Task<RefreshToken> UpdateRefreshTokenLifeTime(Guid token, int userId);
}
