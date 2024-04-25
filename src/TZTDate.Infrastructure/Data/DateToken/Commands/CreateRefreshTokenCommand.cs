using MediatR;
using TZTDate.Core.Data.DateToken.Models;

namespace TZTDate.Infrastructure.Data.DateToken.Commands;

public class CreateRefreshTokenCommand : IRequest<RefreshToken>
{
    public int UserId { get; set; }
    public string CreatedByIp { get; set; }
}
