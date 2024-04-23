using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TZTBank.Core.Data.DateUser.Dtos;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.Core.Data.DateUser.Dtos;
using TZTDate.Infrastructure.Data.DateToken.Commands;
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
    var result = await sender.Send(new LoginCommand() { userLoginDto = loginDto });

    return Ok(result);
  }

  [HttpPut]
  public async Task<IActionResult> UpdateTokenAsync(UpdateTokenDto updateTokenDto)
  {
    var result = await sender.Send(new UpdateTokenCommand { UpdateTokenDto = updateTokenDto });

    if (result == null || !result.Success)
    {
      return BadRequest(result?.ErrorMessage ?? "Failed to update token.");
    }

    return Ok(new
    {
      accessToken = result.AccessToken,
      refreshToken = result.RefreshToken
    });
  }
}
