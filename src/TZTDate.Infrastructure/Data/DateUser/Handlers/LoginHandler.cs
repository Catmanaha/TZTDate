using System.Security.Claims;
using MediatR;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.Core.Data.DateUser;
using TZTDate.Infrastructure.Data;
using TZTDate.Infrastructure.Data.DateUser.Commands;
using TZTDate.Infrastructure.Services.Base;

namespace TZTBank.Infrastructure.Data.BankUser.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, string>
{
    private readonly ITokenService tokenService;
    private readonly ISender sender;
    private readonly TZTDateDbContext context;

    public LoginHandler(ITokenService tokenService, ISender sender, TZTDateDbContext context)
    {
        this.context = context;
        this.sender = sender;
        this.tokenService = tokenService;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (request.userLoginDto is null)
        {
            throw new NullReferenceException($"{nameof(request.userLoginDto)} cannot be null");
        }

        if (string.IsNullOrEmpty(request.userLoginDto.Email))
        {
            throw new NullReferenceException($"{nameof(request.userLoginDto.Email)} cannot be empty");
        }

        if (string.IsNullOrEmpty(request.userLoginDto.Password))
        {
            throw new NullReferenceException($"{nameof(request.userLoginDto.Password)} cannot be empty");
        }

        var user = await sender.Send(new FindByEmailCommand
        {
            Email = request.userLoginDto.Email
        });

        if (user is null)
        {
            throw new NullReferenceException("User email not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.userLoginDto.Password, user.PasswordHash))
        {
            throw new ArgumentException("Wrong password");
        }

        var roles = await sender.Send(new GetUserRolesCommand
        {
            UserId = user.Id
        });

        var claims = new List<Claim>() {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }


        var refreshToken = new RefreshToken()
        {
            Token = Guid.NewGuid(),
            UserId = user.Id
        };

        await this.context.RefreshTokens.AddAsync(refreshToken);
        await this.context.SaveChangesAsync();

        return tokenService.CreateToken(claims);
    }
}
