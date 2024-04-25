using MediatR;
using TZTDate.Core.Data.DateToken.Models;

namespace TZTDate.Infrastructure.Data.DateToken.Commands;

public class GetRefreshTokenCommand : IRequest<RefreshToken>
{
    public Guid refershToken;
    public int userId;
}
