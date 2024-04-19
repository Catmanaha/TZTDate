using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TZTBank.Core.Data.DateUser.Dtos;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.WebApi.Filters;
using TZTDate.WebApi.Options;

namespace TZTDate.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[ServiceFilter(typeof(ValidationFilterAttribute))]
public class AuthController : ControllerBase {
  private readonly JwtOptions jwtOptions;
  private readonly ISender sender;

  public AuthController(ISender sender,
                        IOptionsSnapshot<JwtOptions> jwtOptions) {
    this.sender = sender;
    this.jwtOptions = jwtOptions.Value;
  }

  [HttpPost]
  public async Task<ActionResult> Register(UserRegisterDto userDto) {
    await sender.Send(new AddNewCommand() { UserRegisterDto = userDto });

    return Ok();
  }

  [HttpPost]
  public async Task<ActionResult> Login(UserLoginDto loginDto) {

    await sender.Send(new LoginCommand() { userLoginDto = loginDto });

    var jwt = CreateToken(loginDto);

    return Ok(jwt);
  }

  private string CreateToken(UserLoginDto loginDto) {
    var claims = new List<Claim>() { new(ClaimTypes.Email, loginDto.Email),
                                     new(ClaimTypes.Role, "User") };

    var securityKey = new SymmetricSecurityKey(this.jwtOptions.KeyInBytes);
    var signingCredentials =
        new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var securityToken = new JwtSecurityToken(
        issuer: this.jwtOptions.Issuers.First(),
        audience: this.jwtOptions.Audience, claims,
        expires: DateTime.Now.AddMinutes(this.jwtOptions.LifetimeInMinutes),
        signingCredentials: signingCredentials);

    var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    var jwt = jwtSecurityTokenHandler.WriteToken(securityToken);

    return jwt;
  }
}