using MediatR;
using TZTDate.Core.Data.DateToken.Models;

namespace TZTDate.Infrastructure.Data.DateToken.Commands;

public class UpdateRefreshTokenLifeTimeCommand : IRequest<RefreshToken>
{
    public Guid Token { get; set; }
    public int UserId { get; set; }
}
