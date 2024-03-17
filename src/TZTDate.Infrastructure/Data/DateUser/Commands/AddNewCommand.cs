using MediatR;
using TZTBank.Core.Data.DateUser.Dtos;

namespace TZTBank.Infrastructure.Data.DateUser.Commands;

public class AddNewCommand : IRequest
{
    public UserRegisterDto? UserRegisterDto { get; set; }
}
