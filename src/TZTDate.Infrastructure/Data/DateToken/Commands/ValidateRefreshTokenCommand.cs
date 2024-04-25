using MediatR;
using TZTDate.Core.Data.DateToken.Responses;

namespace TZTDate.Infrastructure.Data.DateToken.Commands;

public class ValidateRefreshTokenCommand : IRequest<RefreshTokenValidationResponse>
{
    public Guid Token { get; set; }
    public int UserId { get; set; }
}
