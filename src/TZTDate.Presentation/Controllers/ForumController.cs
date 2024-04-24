namespace TZTDate.Presentation.Controllers;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TZTDate.Core.Data.DateUser;
using TZTDate.Infrastructure.Data.ChatHub;

public class ForumController : Controller
{
    private readonly UserManager<User> userManager;
    public IHubContext<ChatHub> hubContext;
    public ForumController(UserManager<User> userManager, IHubContext<ChatHub> hubContext )
    {
        this.hubContext = hubContext;
        this.userManager = userManager;

    }
    public async Task<IActionResult> Index()
    {
        var userName = (await userManager.GetUserAsync(User)).UserName;
        return View(model: userName);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}