using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TZTBank.Core.Data.DateUser.Dtos;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.Core.Data.DateUser;
using TZTDate.Infrastructure.Data;

namespace TZTDate.Presentation.Controllers;

public class UserController : Controller
{
    private readonly ISender sender;
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;
    private readonly TZTDateDbContext context;

    public UserController(ISender sender, SignInManager<User> signInManager, UserManager<User> userManager,TZTDateDbContext context)
    {
        this.sender = sender;
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.context = context;
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

        if (userdto.ReturnUrl is null)
        {
            return RedirectToAction("Account", "User");
        }

        return RedirectPermanent(userdto.ReturnUrl);

    }

    [HttpGet]
    public async Task<IActionResult> Account()
    {
        User user = await userManager.GetUserAsync(User);

        return View(user);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }

    [HttpPost]
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        User user = await userManager.GetUserAsync(User);
        var newUserId = user.Id;

        var fileExtension = new FileInfo(file.FileName).Extension;

        var filename = $"{newUserId}{fileExtension}";

        var destinationAvatarPath = $"wwwroot/Assets/{filename}";

        using var fileStream = System.IO.File.Create(destinationAvatarPath);
        await file.CopyToAsync(fileStream);

        User path = await context.Users.FirstOrDefaultAsync(e => e.Id == user.Id);
        path.ProfilePicPath = filename;
        await context.SaveChangesAsync();

        return base.RedirectToAction("Account", "User");
    }
}