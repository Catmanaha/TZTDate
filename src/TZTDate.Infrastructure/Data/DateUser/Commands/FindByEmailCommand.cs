using MediatR;
using TZTDate.Core.Data.DateUser;

namespace TZTDate.Infrastructure.Data.DateUser.Commands;

public class FindByEmailCommand : IRequest<User>
{
    public string Email { get; set; }
}
