using MediatR;
using TZTDate.Core.Data.DateUser;

namespace TZTDate.Infrastructure.Data.DateUser.Commands;

public class GetUserRolesCommand : IRequest<IEnumerable<Role>>
{
    public int UserId { get; set; }
}
