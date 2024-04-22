using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TZTDate.Core.Data.DateUser;
using TZTDate.Core.Data.DateUser.Enums;
using TZTDate.Core.Data.Options;
using TZTDate.Infrastructure.Services.Base;

namespace TZTDate.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions jwtOptions;

        public TokenService(IOptionsSnapshot<JwtOptions> optionsSnapshot)
        {
            this.jwtOptions = optionsSnapshot.Value;

        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>() {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, UserRoles.User.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

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

        public string CreateTokenAdmin(User user)
        {
            var claims = new List<Claim>() {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, UserRoles.Admin.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

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
    }
}