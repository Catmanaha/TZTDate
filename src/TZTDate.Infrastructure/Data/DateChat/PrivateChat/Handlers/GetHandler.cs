namespace TZTDate.Infrastructure.Data.DateChat.PrivateChat.Handlers;

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateChat.Entities;
using TZTDate.Infrastructure.Data.DateChat.PrivateChat.Commands;

public class GetHandler : IRequestHandler<GetCommand, PrivateChat>
{
    private readonly TZTDateDbContext tZTDateDbContext;
    public GetHandler(TZTDateDbContext tZTDateDbContext)
    {
        this.tZTDateDbContext = tZTDateDbContext;

    }
    public async Task<PrivateChat?> Handle(GetCommand request, CancellationToken cancellationToken)
    {
        return await this.tZTDateDbContext.PrivateChats
            .Include(pc => pc.Messages)
            .FirstOrDefaultAsync<PrivateChat>(privateChat =>
                                privateChat.PrivateChatHashName.Contains(request.CurrentUserId)
                                && privateChat.PrivateChatHashName.Contains(request.CompanionUserId)) ?? null;
    }
}
