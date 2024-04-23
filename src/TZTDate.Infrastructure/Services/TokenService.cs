using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TZTDate.Core.Data.DateUser;
using TZTDate.Core.Data.Options;
using TZTDate.Infrastructure.Data;
using TZTDate.Infrastructure.Services.Base;

namespace TZTDate.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions jwtOptions;
    private readonly TZTDateDbContext context;

    public TokenService(IOptionsSnapshot<JwtOptions> optionsSnapshot, TZTDateDbContext context)
    {
        this.context = context;
        this.jwtOptions = optionsSnapshot.Value;

    }

    public string CreateToken(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(this.jwtOptions.KeyInBytes);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.Aes128CbcHmacSha256);

        var securityToken = new JwtSecurityToken(
            issuer: this.jwtOptions.Issuers.First(),
            audience: this.jwtOptions.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(this.jwtOptions.LifetimeInMinutes),
            signingCredentials: signingCredentials
        );

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var jwt = jwtSecurityTokenHandler.WriteToken(securityToken);

        return jwt;
    }

    public async Task<bool> ValidateToken(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();

        var validationResult = await handler.ValidateTokenAsync(
            accessToken,
            new TokenValidationParameters()
            {
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(this.jwtOptions.KeyInBytes),

                ValidateAudience = true,
                ValidAudience = this.jwtOptions.Audience,

                ValidateIssuer = true,
                ValidIssuers = this.jwtOptions.Issuers,
            }
        );

        return validationResult.IsValid;
    }

    public JwtSecurityToken ReadToken(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.ReadJwtToken(accessToken);

        return securityToken;
    }

    public async Task<RefreshToken> UpdateRefreshTokenLifeTime(Guid token, int userId)
    {
        var refreshTokenToChange = this.context.RefreshTokens.FirstOrDefault(refreshToken => refreshToken.Token == token && refreshToken.UserId == userId);

        if (refreshTokenToChange == null)
        {
            var refreshTokensToDelete = await this.context.RefreshTokens.Where(rf => rf.UserId == userId)
                .ToListAsync();

            this.context.RefreshTokens.RemoveRange(refreshTokensToDelete);
            await this.context.SaveChangesAsync();

            throw new ArgumentException($"Refresh token '{token}' doesn't exist for userid '{userId}'");
        }

        refreshTokenToChange.Token = Guid.NewGuid();
        await this.context.SaveChangesAsync();

        return refreshTokenToChange;
    }
}
