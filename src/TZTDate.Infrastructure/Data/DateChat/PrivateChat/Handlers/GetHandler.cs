namespace TZTDate.Infrastructure.Data.DateChat.PrivateChat.Handlers;

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateChat.Entities;
using TZTDate.Infrastructure.Data.DateChat.PrivateChat.Commands;
using TZTDate.Infrastructure.Data.DateUser.Commands;

public class GetHandler : IRequestHandler<GetCommand, PrivateChat>
{
    private readonly TZTDateDbContext tZTDateDbContext;
    private readonly ISender sender;

    public GetHandler(TZTDateDbContext tZTDateDbContext, ISender sender)
    {
        this.tZTDateDbContext = tZTDateDbContext;
        this.sender = sender;
    }

    public async Task<PrivateChat?> Handle(GetCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await sender.Send(new FindByIdCommand { Id = request.CurrentUserId });
        var companionUser = await sender.Send(new FindByIdCommand { Id = request.CompanionUserId });

        var privateChats = await this.tZTDateDbContext.PrivateChats.FirstOrDefaultAsync(o => o.PrivateChatHashName.Contains(currentUser.Email) && o.PrivateChatHashName.Contains(companionUser.Email));
        privateChats.Messages = await this.tZTDateDbContext.Message.Where(m => m.PrivateChatId == privateChats.Id).ToListAsync()
        ;
        return privateChats;
    }
}