using MediatR;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateToken.Models;
using TZTDate.Infrastructure.Data.DateToken.Commands;

namespace TZTDate.Infrastructure.Data.DateToken.Handlers;

public class UpdateRefreshTokenLifeTimeHandler : IRequestHandler<UpdateRefreshTokenLifeTimeCommand, RefreshToken>
{
    private readonly TZTDateDbContext context;
    public UpdateRefreshTokenLifeTimeHandler(TZTDateDbContext context)
    {
        this.context = context;

    }
    public async Task<RefreshToken> Handle(UpdateRefreshTokenLifeTimeCommand request, CancellationToken cancellationToken)
    {
        var refreshTokenToChange = this.context.RefreshTokens.FirstOrDefault(refreshToken => refreshToken.Token == request.Token && refreshToken.UserId == request.UserId);

        if (refreshTokenToChange == null)
        {
            var refreshTokensToDelete = await this.context.RefreshTokens.Where(rf => rf.UserId == request.UserId)
                .ToListAsync();

            this.context.RefreshTokens.RemoveRange(refreshTokensToDelete);
            await this.context.SaveChangesAsync();

            throw new ArgumentException($"Refresh token '{request.Token}' doesn't exist for userid '{request.UserId}'");
        }

        refreshTokenToChange.Token = Guid.NewGuid();
        await this.context.SaveChangesAsync();

        return refreshTokenToChange;
    }
}
