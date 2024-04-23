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
        var refreshToken = await this.context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == request.Token);

        if (refreshToken == null)
        {
            throw new Exception("Invalid token.");
        }

        refreshToken.Revoked = true;
        refreshToken.RevokedByIp = request.RevokedByIp;

        await this.context.SaveChangesAsync();

        var logEntry = new LogEntry
        {
            EventDate = DateTime.UtcNow,
            EventIp = request.RevokedByIp,
            EventUserId = refreshToken.UserId,
            EventType = "RefreshTokenRevoked"
        };

        await sender.Send(new CreateLogEntryCommand
        {
            LogEntry = logEntry
        });
    }
}
