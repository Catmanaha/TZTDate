using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TZTDate.BlogApi.Data;
using TZTDate.BlogApi.Dtos;
using TZTDate.BlogApi.Models;
using TZTDate.BlogApi.Services.Base;

namespace TZTDate.BlogApi.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly BlogDbContext blogDbContext;
    private readonly ITokenService tokenService;

    public UserController(BlogDbContext blogDbContext, ITokenService tokenService)
    {
        this.blogDbContext = blogDbContext;
        this.tokenService = tokenService;
    }

    [HttpPost]
    public async Task<ActionResult<User>> Register(UserDto userDto)
    {
        if (await blogDbContext.Users.FirstOrDefaultAsync(x => x.Username == userDto.Username.ToLower()) is not null)
        {
            return BadRequest("Username already taken");
        }

        var user = new User
        {
            Username = userDto.Username.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
        };

        await blogDbContext.Users.AddAsync(user);
        await blogDbContext.SaveChangesAsync();

        return user;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Login(UserDto userDto)
    {
        var user = await blogDbContext.Users.FirstOrDefaultAsync(x => x.Username == userDto.Username.ToLower());

        if (user is null)
        {
            return BadRequest($"User doesnt exist with '{userDto}' username");
        }

        if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
        {
            return BadRequest("Wrong password");
        }

        if (userDto.Username.ToLower().Contains("admin"))
        {
            return tokenService.CreateTokenAdmin(user);
        }

        return tokenService.CreateToken(user);
    }

}
