namespace TZTDate.Presentation.Controllers;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TZTDate.Core.Data.DateUser;

public class ChatController : Controller
{
    private readonly UserManager<User> userManager;

    public ChatController(UserManager<User> userManager)
    {
        this.userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
