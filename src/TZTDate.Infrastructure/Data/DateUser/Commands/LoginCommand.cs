using MediatR;
using TZTBank.Core.Data.DateUser.Dtos;

namespace TZTBank.Infrastructure.Data.DateUser.Commands;

public class LoginCommand : IRequest
{
    public UserLoginDto? userLoginDto { get; set; }
}
