using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TZTBank.Core.Data.DateUser.Dtos;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.Infrastructure.Data.DateUser;

namespace TZTDate.Presentation.Controllers;

public class UserController : Controller
{
    private readonly ISender sender;
    private readonly SignInManager<User> signInManager;

    public UserController(ISender sender, SignInManager<User> signInManager)
    {
        this.sender = sender;
        this.signInManager = signInManager;
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterDto userDto)
    {
        if (ModelState.IsValid == false)
        {
            return View();
        }

        try
        {
            await sender.Send(new AddNewCommand()
            {
                UserRegisterDto = userDto
            });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }

        return RedirectToAction("Login");
    }

    public IActionResult Login(string? ReturnUrl)
    {
        ViewData["ReturnUrl"] = ReturnUrl;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginDto userdto)
    {
        if (ModelState.IsValid == false)
        {
            return View();
        }

        try
        {
            await sender.Send(new LoginCommand()
            {
                userLoginDto = userdto
            });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Error", ex.Message);
            return View();
        }

        if (userdto.ReturnUrl is null) {
            return RedirectToAction("GetAll", "Profile");
        }

        return RedirectPermanent(userdto.ReturnUrl);

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
