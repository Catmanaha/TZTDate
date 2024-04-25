using System.IdentityModel.Tokens.Jwt;
using MediatR;
using TZTDate.Infrastructure.Data.DateToken.Commands;

namespace TZTDate.Infrastructure.Data.DateToken.Handlers;

public class ReadTokenHandler : IRequestHandler<ReadTokenCommand, JwtSecurityToken>
{
    public async Task<JwtSecurityToken> Handle(ReadTokenCommand request, CancellationToken cancellationToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.ReadJwtToken(request.AccessToken);

        return securityToken;
    }
}
