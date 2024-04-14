using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TZTBank.Core.Data.DateUser.Dtos;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.WebApi.Options;

namespace TZTDate.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly JwtOptions jwtOptions;
    private readonly ISender sender;

    public AuthController(ISender sender, IOptionsSnapshot<JwtOptions> JwtOptions)
    {
        this.sender = sender;
    }

    [HttpPost]
    public async Task<ActionResult> Register(UserRegisterDto userDto)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest();
        }
        try
        {
            await sender.Send(new AddNewCommand()
            {
                UserRegisterDto = userDto
            });

            return Ok();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<string> Login(UserLoginDto loginDto)
    {
        if (ModelState.IsValid == false)
        {
            return null;
        }

        try
        {
            await sender.Send(new LoginCommand()
            {
                userLoginDto = loginDto
            });

            var claims = new List<Claim>() {
                new(ClaimTypes.Name, loginDto.Email),
            };

            var securityKey = new SymmetricSecurityKey(this.jwtOptions.KeyInBytes);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

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
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return null;
        }
    }
}