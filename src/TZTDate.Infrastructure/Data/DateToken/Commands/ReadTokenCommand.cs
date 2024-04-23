using System.IdentityModel.Tokens.Jwt;
using MediatR;

namespace TZTDate.Infrastructure.Data.DateToken.Commands;

public class ReadTokenCommand : IRequest<JwtSecurityToken>
{
    public string AccessToken { get; set; }
}
