using MediatR;
using Microsoft.AspNetCore.Identity;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.Core.Data.DateUser.Enums;
using TZTDate.Infrastructure.Data.DateUser;

namespace TZTBank.Infrastructure.Data.BankUser.Handlers;

public class AddNewHandler : IRequestHandler<AddNewCommand>
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public AddNewHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task Handle(AddNewCommand request, CancellationToken cancellationToken)
    {
        if (request.UserRegisterDto is null) {
            throw new NullReferenceException($"{nameof(request.UserRegisterDto)} cannot be null");
        }

        var user = new User
        {
            UserName = request.UserRegisterDto.Username,
            Email = request.UserRegisterDto.Email,
            Age = request.UserRegisterDto.Age,
            CreatedAt = DateTime.Now,
            Description = request.UserRegisterDto.Description,
            Location = request.UserRegisterDto.Location,
            Gender = request.UserRegisterDto.Gender,
            ProfilePicPath = request.UserRegisterDto.ProfilePicPath
        };

        var result = await userManager.CreateAsync(user, request.UserRegisterDto.Password);
        
        if (!result.Succeeded)
        {

            var errors = new List<Exception>();

            foreach (var error in result.Errors)
            {
                errors.Add(new ArgumentException(error.Description, error.Code));
            }

            if (errors.Any())
            {
                throw new AggregateException(errors);
            }


            var userRole = new IdentityRole
            {
                Name = UserRoles.User.ToString()
            };

            await roleManager.CreateAsync(userRole);
            await userManager.AddToRoleAsync(user, UserRoles.User.ToString());

        }
    }
}
