namespace TZTDate.Infrastructure.Data.DateChat.PrivateChat.Handlers;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateUser.Chat;
using TZTDate.Infrastructure.Data.DateChat.PrivateChat.Commands;

public class GetHandler : IRequestHandler<GetCommand, PrivateChat>
{
    private readonly TZTDateDbContext tZTDateDbContext;
    public GetHandler(TZTDateDbContext tZTDateDbContext)
    {
        this.tZTDateDbContext = tZTDateDbContext;

    }
    public async Task<PrivateChat> Handle(GetCommand request, CancellationToken cancellationToken)
    {
        return await this.tZTDateDbContext.PrivateChats
            .FirstOrDefaultAsync<PrivateChat>(privateChat => 
                                privateChat.PrivateChatHashName.Contains(request.CurrentUserId) 
                                && privateChat.PrivateChatHashName.Contains(request.CompanionUserId));
    }
}
