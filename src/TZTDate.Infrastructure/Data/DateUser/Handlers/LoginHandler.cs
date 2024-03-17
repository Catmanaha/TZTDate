using MediatR;
using Microsoft.AspNetCore.Identity;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.Core.Data.DateUser;

namespace TZTBank.Infrastructure.Data.BankUser.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand>
{
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;

    public LoginHandler(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    public async Task Handle(LoginCommand request, CancellationToken cancellationToken)
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

        var user = await userManager.FindByEmailAsync(request.userLoginDto.Email);

        if (user is null)
        {
            throw new NullReferenceException("User email not found");
        }

        var result = await signInManager.PasswordSignInAsync(user, request.userLoginDto.Password, true, true);

        if (result.Succeeded == false)
        {
            throw new ArgumentNullException("Incorrect Credentials");
        }

    }
}
