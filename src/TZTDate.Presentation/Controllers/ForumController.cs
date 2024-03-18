namespace TZTDate.Presentation.Controllers;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TZTDate.Core.Data.DateUser;

public class ForumController : Controller
{
    private readonly UserManager<User> userManager;
    public ForumController(UserManager<User> userManager)
    {
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
