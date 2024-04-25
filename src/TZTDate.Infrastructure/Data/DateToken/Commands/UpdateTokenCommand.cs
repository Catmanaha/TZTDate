using MediatR;
using TZTDate.Core.Data.DateToken.Responses;
using TZTDate.Core.Data.DateUser.Dtos;

namespace TZTDate.Infrastructure.Data.DateToken.Commands;

public class UpdateTokenCommand : IRequest<UpdateTokenResponse>
{
    public UpdateTokenDto UpdateTokenDto { get; set; }
}
