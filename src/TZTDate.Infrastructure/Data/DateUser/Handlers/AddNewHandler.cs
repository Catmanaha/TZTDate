using MediatR;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.Core.Data.DateUser;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using TZTDate.Infrastructure.Data.DateUser.Commands;
using Microsoft.EntityFrameworkCore;
using TZTDate.Infrastructure.Services.Base;
using TZTDate.Core.Data.DateUser.Enums;

namespace TZTDate.Infrastructure.Data.DateUser.Handlers;

public class AddNewHandler : IRequestHandler<AddNewCommand>
{
    private readonly TZTDateDbContext tZTDateDbContext;
    private readonly ISender sender;
    private readonly IAzureBlobService azureBlobService;

    public AddNewHandler(TZTDateDbContext tZTDateDbContext, ISender sender, IAzureBlobService azureBlobService)
    {
        this.azureBlobService = azureBlobService;
        this.tZTDateDbContext = tZTDateDbContext;
        this.sender = sender;
    }

    public async Task Handle(AddNewCommand request, CancellationToken cancellationToken)
    {
        if (request.UserRegisterDto is null)
        {
            throw new ArgumentNullException($"{nameof(request.UserRegisterDto)} cannot be null");
        }

        var result = await sender.Send(new FindByEmailCommand
        {
            Email = request.UserRegisterDto.Email
        });

        if (result is not null)
        {
            throw new ArgumentNullException($"{request.UserRegisterDto.Email} already exists");
        }

        var address = new Address
        {
            City = request.UserRegisterDto.City,
            Country = request.UserRegisterDto.Country,
            State = request.UserRegisterDto.State,
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

                    await azureBlobService.Uploadfile(formFile, filename);

                    imagePaths.Add(filename);
                }
            }
        }

        if (await tZTDateDbContext.Users.FirstOrDefaultAsync(x => x.Username == request.UserRegisterDto.Username) is not null)
        {
            throw new ArgumentException("Username already taken");
        }

        var user = new User
        {
            Username = request.UserRegisterDto.Username,
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
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.UserRegisterDto.Password),
        };

        await tZTDateDbContext.Users.AddAsync(user);
        await tZTDateDbContext.SaveChangesAsync();

        var role = await tZTDateDbContext.Roles.FirstOrDefaultAsync(r => r.Name == UserRoles.User.ToString());
        if (role == null)
        {
            throw new ArgumentException($"Role does not exist with {UserRoles.User} name");
        }

        var userRole = new UserRole { User = user, Role = role };
        await tZTDateDbContext.UserRoles.AddAsync(userRole);

        await tZTDateDbContext.SaveChangesAsync();
    }
}