using MediatR;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateLogEntry.Models;
using TZTDate.Infrastructure.Data.DateLogEntry.Commands;
using TZTDate.Infrastructure.Data.DateToken.Commands;

namespace TZTDate.Infrastructure.Data.DateToken.Handlers;

public class RevokeRefreshTokenHandler : IRequestHandler<RevokeRefreshTokenCommand>
{
    private readonly ISender sender;
    private readonly TZTDateDbContext context;

    public RevokeRefreshTokenHandler(ISender sender, TZTDateDbContext context)
    {
        this.sender = sender;
        this.context = context;
    }
    public async Task Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var oldRefreshToken = await sender.Send(new GetRefreshTokenCommand
        {
            refershToken = request.Token,
            userId = request.UserId
        });

        if (oldRefreshToken == null)
        {
            throw new Exception("Invalid token.");
        }

        oldRefreshToken.Revoked = true;
        oldRefreshToken.RevokedByIp = request.RevokedByIp;
        oldRefreshToken.ReplacedByTokenId = request.RefreshTokenReplacedById;

        await this.context.SaveChangesAsync();

        var logEntry = new LogEntry
        {
            EventDate = DateTime.UtcNow,
            EventIp = request.RevokedByIp,
            EventUserId = oldRefreshToken.UserId,
            EventType = "RefreshTokenRevoked"
        };

        await sender.Send(new CreateLogEntryCommand
        {
            LogEntry = logEntry
        });
    }
}
