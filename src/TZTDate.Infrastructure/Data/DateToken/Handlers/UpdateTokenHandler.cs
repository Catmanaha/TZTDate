using System.Security.Claims;
using MediatR;
using TZTDate.Core.Data.DateToken.Responses;
using TZTDate.Infrastructure.Data.DateToken.Commands;
using TZTDate.Infrastructure.Data.DateUser.Commands;

namespace TZTDate.Infrastructure.Data.DateToken.Handlers;

public class UpdateTokenHandler : IRequestHandler<UpdateTokenCommand, UpdateTokenResponse>
{
    private readonly ISender sender;

    public UpdateTokenHandler(ISender sender)
    {
        this.sender = sender;
    }

    public async Task<UpdateTokenResponse> Handle(UpdateTokenCommand request, CancellationToken cancellationToken)
    {
        var securityToken = await sender.Send(new ReadTokenCommand
        {
            AccessToken = request.UpdateTokenDto.AccessToken
        });

        var idClaim = securityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (idClaim == null)
        {
            return new UpdateTokenResponse
            {
                Success = false,
                ErrorMessage = $"JWT Token must contain '{ClaimTypes.NameIdentifier}' claim!"
            };
        }

        int id = int.Parse(idClaim.Value);
        var user = await sender.Send(new FindByIdCommand
        {
            Id = id
        });

        if (user == null)
        {
            return new UpdateTokenResponse
            {
                Success = false,
                ErrorMessage = $"Couldn't update the token. User with id '{id}' doesn't exist!"
            };
        }

        var validateRefreshToken = await sender.Send(new ValidateRefreshTokenCommand
        {
            Token = request.UpdateTokenDto.RefreshToken,
            UserId = id
        });

        if (validateRefreshToken.IsValid == false)
        {
            return new UpdateTokenResponse
            {
                Success = false,
                ErrorMessage = validateRefreshToken.Message
            };
        }

        var roles = await sender.Send(new GetUserRolesCommand
        {
            UserId = user.Id
        });

        var claims = roles
            .Select(role => new Claim(ClaimTypes.Role, role.Name))
            .Append(new Claim(ClaimTypes.Name, user.Username))
            .Append(new Claim(ClaimTypes.Email, user.Email))
            .Append(new Claim(ClaimTypes.NameIdentifier, id.ToString()));

        var newJwt = await sender.Send(new CreateTokenCommand
        {
            Claims = claims
        });

        var updatedRefreshToken = await sender.Send(new UpdateRefreshTokenLifeTimeCommand
        {
            Token = request.UpdateTokenDto.RefreshToken,
            UserId = id
        });

        await sender.Send(new RevokeRefreshTokenCommand
        {
            Token = request.UpdateTokenDto.RefreshToken,
            RevokedByIp = "System"
        });

        return new UpdateTokenResponse
        {
            AccessToken = newJwt,
            RefreshToken = updatedRefreshToken.Token,
            Success = true
        };
    }
}
