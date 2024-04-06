using MediatR;
using Microsoft.AspNetCore.Identity;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.Core.Data.DateUser.Enums;
using TZTDate.Core.Data.DateUser;
using TZTDate.Infrastructure.Data;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace TZTBank.Infrastructure.Data.BankUser.Handlers;

public class AddNewHandler : IRequestHandler<AddNewCommand>
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly TZTDateDbContext tZTDateDbContext;

    public AddNewHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, TZTDateDbContext tZTDateDbContext)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.tZTDateDbContext = tZTDateDbContext;
    }

    public async Task Handle(AddNewCommand request, CancellationToken cancellationToken)
    {
        if (request.UserRegisterDto is null)
        {
            throw new NullReferenceException($"{nameof(request.UserRegisterDto)} cannot be null");
        }

        var address = new Address
        {
            City = request.UserRegisterDto.City,
            Country = request.UserRegisterDto.Country,
            District = request.UserRegisterDto.District,
            Latitude = request.UserRegisterDto.Latitude,
            Longitude = request.UserRegisterDto.Longitude,
            PostCode = request.UserRegisterDto.PostCode,
            Street = request.UserRegisterDto.Street
        };

        await tZTDateDbContext.Addresses.AddAsync(address);

        var imagePaths = new List<string>();

        var type = request.UserRegisterDto.GetType();
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            if (property.Name.Contains("Image"))
            {
                if (property.GetValue(request.UserRegisterDto) is IFormFile formFile)
                {
                    var fileExtension = new FileInfo(formFile.FileName).Extension;

                    var filename = $"{Guid.NewGuid()}{fileExtension}";

                    var destinationAvatarPath = $"wwwroot/Assets/{filename}";

                    using var fileStream = File.Create(destinationAvatarPath);
                    await formFile.CopyToAsync(fileStream);

                    imagePaths.Add(filename);
                }
            }
        }

        var user = new User
        {
            UserName = request.UserRegisterDto.Username,
            Email = request.UserRegisterDto.Email,
            BirthDateTime = request.UserRegisterDto.BirthDateTime,
            CreatedAt = DateTime.Now,
            Description = request.UserRegisterDto.Description,
            Address = address,
            Gender = request.UserRegisterDto.Gender,
            ProfilePicPaths = imagePaths.ToArray(),
            SearchingGender = request.UserRegisterDto.SearchingGender,
            SearchingAgeStart = request.UserRegisterDto.SearchingAgeStart,
            SearchingAgeEnd = request.UserRegisterDto.SearchingAgeEnd,
            Interests = request.UserRegisterDto.Interests,
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
