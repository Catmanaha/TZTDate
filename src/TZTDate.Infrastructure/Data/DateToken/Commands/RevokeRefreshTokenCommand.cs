using MediatR;

namespace TZTDate.Infrastructure.Data.DateToken.Commands;

public class RevokeRefreshTokenCommand : IRequest
{
    public Guid Token { get; set; }
    public string RevokedByIp { get; set; }
}
