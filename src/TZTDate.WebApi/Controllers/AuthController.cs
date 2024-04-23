using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TZTBank.Core.Data.DateUser.Dtos;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.Core.Data.DateUser.Dtos;
using TZTDate.Infrastructure.Data.DateUser.Commands;
using TZTDate.Infrastructure.Services.Base;
using TZTDate.WebApi.Filters;

namespace TZTDate.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[ServiceFilter(typeof(ValidationFilterAttribute))]
public class AuthController : ControllerBase
{
  private readonly ISender sender;
  private readonly ITokenService tokenService;

  public AuthController(ISender sender, ITokenService tokenService)
  {
    this.tokenService = tokenService;
    this.sender = sender;
  }

  [HttpPost]
  public async Task<ActionResult> Register([FromForm] UserRegisterDto userDto)
  {
    await sender.Send(new AddNewCommand() { UserRegisterDto = userDto });

    return Ok();
  }

  [HttpPost]
  public async Task<ActionResult> Login(UserLoginDto loginDto)
  {
    var jwt = await sender.Send(new LoginCommand() { userLoginDto = loginDto });

    return Ok(jwt);
  }

  [HttpPut]
  public async Task<IActionResult> UpdateTokenAsync(UpdateTokenDto updateTokenDto)
  {
    var validateToken = await tokenService.ValidateToken(updateTokenDto.AccessToken);

    if (validateToken == false)
    {
      return base.BadRequest("Token is invalid!");
    }

    var securityToken = tokenService.ReadToken(updateTokenDto.AccessToken);
    var idClaim = securityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

    if (idClaim == null)
    {
      return base.BadRequest("JWT Token must contain 'id' claim!");
    }

    int id = int.Parse(idClaim.Value);
    var user = await sender.Send(new FindByIdCommand
    {
      Id = id
    });

    if (user == null)
    {
      return base.NotFound($"Couldn't update the token. User with id '{id}' doesn't exist!");
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

    var updatedRefreshToken = await tokenService.UpdateRefreshTokenLifeTime(updateTokenDto.RefreshToken, id);

    return base.Ok(new
    {
      accessToken = newJwt,
      refreshToken = updatedRefreshToken.Token
    });
  }

}
