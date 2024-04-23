using System.Security.Claims;
using MediatR;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.Core.Data.DateUser;
using TZTDate.Core.Data.DateUser.Responses;
using TZTDate.Infrastructure.Data;
using TZTDate.Infrastructure.Data.DateUser.Commands;
using TZTDate.Infrastructure.Services.Base;

namespace TZTBank.Infrastructure.Data.BankUser.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
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

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (request.userLoginDto is null)
        {
            throw new ArgumentNullException($"{nameof(request.userLoginDto)} cannot be null");
        }

        if (string.IsNullOrEmpty(request.userLoginDto.Email))
        {
            throw new ArgumentNullException($"{nameof(request.userLoginDto.Email)} cannot be empty");
        }

        if (string.IsNullOrEmpty(request.userLoginDto.Password))
        {
            throw new ArgumentNullException($"{nameof(request.userLoginDto.Password)} cannot be empty");
        }

        var user = await sender.Send(new FindByEmailCommand
        {
            Email = request.userLoginDto.Email
        });

        if (user is null)
        {
            throw new ArgumentNullException("User email not found");
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

        var refreshToken = await tokenService.CreateRefreshToken(user.Id, request.userLoginDto.IpAddress);

        return new LoginResponse{
            AccessToken = tokenService.CreateToken(claims),
            RefreshToken = refreshToken.Token
        };
    }
}
