using MediatR;
using TZTDate.Core.Data.DateUser.Dtos;
using TZTDate.Infrastructure.Data.DateToken.Responses;

namespace TZTDate.Infrastructure.Data.DateToken.Commands;

public class UpdateTokenCommand : IRequest<UpdateTokenResponse>
{
    public UpdateTokenDto UpdateTokenDto { get; set; }
}
