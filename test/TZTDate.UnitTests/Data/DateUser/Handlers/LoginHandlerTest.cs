using Microsoft.AspNetCore.Identity;
using Moq;
using TZTBank.Core.Data.DateUser.Dtos;
using TZTBank.Infrastructure.Data.BankUser.Handlers;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.Core.Data.DateUser;

namespace TZTDate.UnitTests.Data.DateUser.Handlers;

public class LoginHandlerTest
{
    [Fact]
    public async Task Handle_UserDtoNull_ThrowNullReferenceException()
    {
        var handler = new LoginHandler(null, null);

        await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(null, new CancellationToken()));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", null)]
    [InlineData(null, null)]
    [InlineData(null, "")]
    [InlineData("email", "")]
    [InlineData("", "password")]
    public async Task Handle_DtoDataNullOrEmpty_ThrowNullReferenceException(string? email, string? password)
    {
        var handler = new LoginHandler(null, null);

        var command = new LoginCommand
        {
            userLoginDto = new UserLoginDto
            {
                Email = email,
                Password = password
            }
        };

        await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(command, new CancellationToken()));
    }

    [Fact]
    public async Task Handle_UserNull_ThrowNullReferenceException()
    {
        var userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

        var handler = new LoginHandler(userManager.Object, null);

        var command = new LoginCommand
        {
            userLoginDto = new UserLoginDto
            {
                Email = "email",
                Password = "password"
            }
        };

        userManager.Setup(repo => repo.FindByEmailAsync(null)).ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<NullReferenceException>(() => handler.Handle(command, new CancellationToken()));
    }
}