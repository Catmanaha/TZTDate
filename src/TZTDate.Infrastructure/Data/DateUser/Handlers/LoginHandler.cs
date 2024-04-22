using MediatR;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.Infrastructure.Data.DateUser.Commands;
using TZTDate.Infrastructure.Services.Base;

namespace TZTBank.Infrastructure.Data.BankUser.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, string>
{
    private readonly ITokenService tokenService;
    private readonly ISender sender;

    public LoginHandler(ITokenService tokenService, ISender sender)
    {
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

        if (request.userLoginDto.Email.ToLower().Contains("admin"))
        {
            return tokenService.CreateTokenAdmin(user);
        }

        return tokenService.CreateToken(user);

    }
}
