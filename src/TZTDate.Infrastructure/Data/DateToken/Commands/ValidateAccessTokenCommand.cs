using MediatR;

namespace TZTDate.Infrastructure.Data.DateToken.Commands;

public class ValidateAccessTokenCommand : IRequest<bool>
{
    public string AccessToken { get; set; }
}
