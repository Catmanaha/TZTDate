using MediatR;
using TZTBank.Core.Data.DateUser.Dtos;

namespace TZTBank.Infrastructure.Data.DateUser.Commands;

public class LoginCommand : IRequest<string>
{
    public UserLoginDto? userLoginDto { get; set; }
}
