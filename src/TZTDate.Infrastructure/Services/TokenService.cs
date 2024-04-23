using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TZTDate.Core.Data.DateLogEntry.Models;
using TZTDate.Core.Data.DateUser;
using TZTDate.Core.Data.DateUser.Responses;
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

    public async Task<bool> ValidateAccessToken(string accessToken)
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

    public async Task<RefreshTokenValidationResponse> ValidateRefreshToken(Guid token, int userId)
    {
        var refreshToken = await this.context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);

        if (refreshToken == null)
        {
            return new RefreshTokenValidationResponse
            {
                IsValid = false,
                Message = "Token does not exist"
            };
        }

        if (refreshToken.UserId != userId)
        {
            return new RefreshTokenValidationResponse
            {
                Message = "Token does not belong to this user"
            };
        }

        if (refreshToken.Revoked)
        {
            return new RefreshTokenValidationResponse
            {
                Message = "Token has been revoked"
            };
        }

        if (refreshToken.ExpiryDate < DateTime.UtcNow)
        {
            return new RefreshTokenValidationResponse
            {
                IsValid = false,
                Message = "Token has expired"
            };
        }

        return new RefreshTokenValidationResponse
        {
            IsValid = true
        };
    }

    public JwtSecurityToken ReadToken(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.ReadJwtToken(accessToken);

        return securityToken;
    }

    public async Task<RefreshToken> CreateRefreshToken(int userId, string createdByIp)
    {
        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Token = Guid.NewGuid(),
            ExpiryDate = DateTime.UtcNow.AddHours(jwtOptions.RefreshTokenLifetimeInHours),
            CreatedDate = DateTime.UtcNow,
            CreatedByIp = createdByIp,
            Revoked = false
        };

        await this.context.RefreshTokens.AddAsync(refreshToken);
        await this.context.SaveChangesAsync();

        return refreshToken;
    }

    public async Task RevokeRefreshToken(Guid token, string revokedByIp)
    {
        var refreshToken = await this.context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);

        if (refreshToken == null)
        {
            throw new Exception("Invalid token.");
        }

        refreshToken.Revoked = true;
        refreshToken.RevokedByIp = revokedByIp;

        await this.context.SaveChangesAsync();

        var logEntry = new LogEntry
        {
            EventDate = DateTime.UtcNow,
            EventIp = revokedByIp,
            EventUserId = refreshToken.UserId,
            EventType = "RefreshTokenRevoked"
        };
        await this.context.LogEntries.AddAsync(logEntry);
        await this.context.SaveChangesAsync();
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
