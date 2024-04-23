using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TZTDate.Core.Data.DateUser;
using TZTDate.Core.Data.DateUser.Responses;

namespace TZTDate.Infrastructure.Services.Base;

public interface ITokenService
{
    public string CreateToken(IEnumerable<Claim> claims);
    public Task<bool> ValidateAccessToken(string accessToken);
    public Task<RefreshTokenValidationResponse> ValidateRefreshToken(Guid token, int userId);
    public JwtSecurityToken ReadToken(string accessToken);
    public Task<RefreshToken> UpdateRefreshTokenLifeTime(Guid token, int userId);
    public Task RevokeRefreshToken(Guid token, string revokedByIp);
    public Task<RefreshToken> CreateRefreshToken(int userId, string createdByIp);
}
