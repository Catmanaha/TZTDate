using MediatR;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateToken.Models;
using TZTDate.Infrastructure.Data.DateToken.Commands;

namespace TZTDate.Infrastructure.Data.DateToken.Handlers;

public class GetRefreshTokenHandler : IRequestHandler<GetRefreshTokenCommand, RefreshToken>
{
    private readonly TZTDateDbContext context;
    public GetRefreshTokenHandler(TZTDateDbContext context)
    {
        this.context = context;

    }
    public async Task<RefreshToken> Handle(GetRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(o => o.Token == request.refershToken && o.UserId == request.userId);
    }
}