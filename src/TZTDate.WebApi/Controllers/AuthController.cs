using MediatR;
using Microsoft.AspNetCore.Mvc;
using TZTBank.Core.Data.DateUser.Dtos;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.WebApi.Filters;

namespace TZTDate.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[ServiceFilter(typeof(ValidationFilterAttribute))]
public class AuthController : ControllerBase
{
  private readonly ISender sender;

  public AuthController(ISender sender)
  {
    this.sender = sender;
  }

  [HttpPost]
  public async Task<ActionResult> Register([FromForm]UserRegisterDto userDto)
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

}
