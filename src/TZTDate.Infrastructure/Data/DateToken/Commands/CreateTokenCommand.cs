using System.Security.Claims;
using MediatR;

namespace TZTDate.Infrastructure.Data.DateToken.Commands;

public class CreateTokenCommand : IRequest<string>
{
    public IEnumerable<Claim> Claims { get; set; }
}
