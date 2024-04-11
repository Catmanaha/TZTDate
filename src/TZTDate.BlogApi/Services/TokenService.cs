using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TZTDate.BlogApi.Models;
using TZTDate.BlogApi.Models.Managers;
using TZTDate.BlogApi.Services.Base;

namespace TZTDate.BlogApi.Services;

public class TokenService : ITokenService
{
    private readonly JwtManager jwtManager;

    public TokenService(IOptionsSnapshot<JwtManager> optionsSnapshot)
    {
        this.jwtManager = optionsSnapshot.Value;

    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>() {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "User"),
        };

        var securityKey = new SymmetricSecurityKey(this.jwtManager.KeyInBytes);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.Aes128CbcHmacSha256);

        var securityToken = new JwtSecurityToken(
            issuer: this.jwtManager.Issuer,
            audience: this.jwtManager.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(this.jwtManager.LifetimeInMinutes),
            signingCredentials: signingCredentials
        );

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var jwt = jwtSecurityTokenHandler.WriteToken(securityToken);

        return jwt;
    }

    public string CreateTokenAdmin(User user)
    {
        var claims = new List<Claim>() {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin"),
        };

        var securityKey = new SymmetricSecurityKey(this.jwtManager.KeyInBytes);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.Aes128CbcHmacSha256);

        var securityToken = new JwtSecurityToken(
            issuer: this.jwtManager.Issuer,
            audience: this.jwtManager.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(this.jwtManager.LifetimeInMinutes),
            signingCredentials: signingCredentials
        );

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var jwt = jwtSecurityTokenHandler.WriteToken(securityToken);

        return jwt;
    }
}
