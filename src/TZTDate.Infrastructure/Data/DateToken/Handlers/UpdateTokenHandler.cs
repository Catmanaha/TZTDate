using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using TZTDate.Infrastructure.Data.DateToken.Commands;
using TZTDate.Infrastructure.Data.DateToken.Responses;
using TZTDate.Infrastructure.Data.DateUser.Commands;
using TZTDate.Infrastructure.Services.Base;

namespace TZTDate.Infrastructure.Data.DateToken.Handlers;

public class UpdateTokenHandler : IRequestHandler<UpdateTokenCommand, UpdateTokenResponse>
{
    private readonly ITokenService tokenService;
    private readonly ISender sender;

    public UpdateTokenHandler(ITokenService tokenService, ISender sender)
    {
        this.tokenService = tokenService;
        this.sender = sender;
    }

    public async Task<UpdateTokenResponse> Handle(UpdateTokenCommand request, CancellationToken cancellationToken)
    {
        // The body of your UpdateTokenAsync method goes here.
        // Replace references to updateTokenDto with request.UpdateTokenDto.
        // Return an UpdateTokenResult instead of an IActionResult.

        var securityToken = tokenService.ReadToken(request.UpdateTokenDto.AccessToken);
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

        var validateRefreshToken = await tokenService.ValidateRefreshToken(request.UpdateTokenDto.RefreshToken, id);

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

        var newJwt = tokenService.CreateToken(claims);

        var updatedRefreshToken = await tokenService.UpdateRefreshTokenLifeTime(request.UpdateTokenDto.RefreshToken, id);
        await tokenService.RevokeRefreshToken(request.UpdateTokenDto.RefreshToken, "System");

        return new UpdateTokenResponse
        {
            AccessToken = newJwt,
            RefreshToken = updatedRefreshToken.Token,
            Success = true
        };
    }
}
