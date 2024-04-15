using MediatR;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TZTDate.Core.Data.DateUser;
using TZTDate.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TZTBank.Core.Data.DateUser.Dtos;
using TZTDate.Core.Data.DateUser.Enums;
using Microsoft.AspNetCore.Authorization;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.Core.Data.FaceDetectionApi.Repositories;
using TZTDate.Core.Data.DateUser.FollowMembersViewModel;
using TZTDate.Infrastructure.Data.DateUser.Commands;
using TZTDate.Core.Data.SearchData;
using TZTDate.Infrastructure.Data.SearchData.Services;

namespace TZTDate.Presentation.Controllers;

public class UserController : Controller
{
    private const int pageItemsCount = 12;
    private readonly ISender sender;
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;
    private readonly TZTDateDbContext context;
    private readonly IFaceDetectionRepository faceDetectionRepository;

    public UserController(ISender sender,
                          SignInManager<User> signInManager,
                          UserManager<User> userManager,
                          TZTDateDbContext context,
                          IFaceDetectionRepository faceDetectionRepository
    )
    {
        this.sender = sender;
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.context = context;
        this.faceDetectionRepository = faceDetectionRepository;
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return RedirectToAction("Main", "Home");
    }

    [HttpGet]
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

        return RedirectToAction("Login", "User");
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
            return RedirectToAction("Index", "Home");
        }

        return RedirectPermanent(userdto.ReturnUrl);
    }

    [HttpGet]
    public async Task<IActionResult> Account()
    {
        User user = await userManager.GetUserAsync(User);
        var userWithAddress = await userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(o => o.Id == user.Id);

        return View(userWithAddress);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }

    [HttpPost]
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        // var detect = await faceDetectionRepository.Detect(file);
        // if (!detect) {
        //     TempData["ImageError"] = "Image must contain your face";
        //     return RedirectToAction("Account");
        // }

        // User user = await userManager.GetUserAsync(User);
        // var newUserId = user.Id;

        // var fileExtension = new FileInfo(file.FileName).Extension;

        // var filename = $"{newUserId}{fileExtension}";

        // var destinationAvatarPath = $"wwwroot/Assets/{filename}";

        // using var fileStream = System.IO.File.Create(destinationAvatarPath);
        // await file.CopyToAsync(fileStream);

        // User path = await context.Users.FirstOrDefaultAsync(e => e.Id == user.Id);
        // path.ProfilePicPath = filename;
        // await context.SaveChangesAsync();

        return base.RedirectToAction("Account", "User");
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == id);

        return View(user);
    }

    [HttpGet]
    public async Task<IActionResult> Profiles(string? searchByName, int? startAge, int? endAge, string? interests, Gender? searchGender)
    {
        User me = await userManager.GetUserAsync(User);

        var users = await context.Users.ToListAsync();

        SearchData searchData = new SearchData() {
            Me = me,
            Users = users,
            SearchingGender = searchGender,
            SearchingStartAge = startAge,
            SearchingEndAge = endAge,
            SearchingInterests = interests,
            SearchingUsername = searchByName
        };

        users = SearchDataService.ProfilesFilter(searchData);

        ViewData["SearchingStartAge"] = me.SearchingAgeStart;
        ViewData["SearchingEndAge"] = me.SearchingAgeEnd;
        ViewData["SearchingGender"] = me.SearchingGender.ToString();

        return View(users.GetRange(0, users.Count() < pageItemsCount ? users.Count() : pageItemsCount));
    }

    public async Task<IActionResult> LoadMoreProfiles(int skip, string? searchByName, int? startAge, int? endAge, string? interests, string? searchGender)
    {
        var me = await userManager.GetUserAsync(User);
        var users = await context.Users.ToListAsync();

        users = SearchDataService.MoreProfilesFilter(new SearchData() {
            Me = me,
            Users = users,
            SearchingGender = searchGender == "0" ? Gender.Male : Gender.Female,
            SearchingStartAge = startAge,
            SearchingEndAge = endAge,
            SearchingInterests = interests,
            SearchingUsername = searchByName
        });

        users = users.GetRange(skip, users.Count() - skip < pageItemsCount ? users.Count() - skip : pageItemsCount);

        return PartialView("ProfilesPartial", users);
    }

    [HttpGet]
    [Route("/id")]
    public async Task<IActionResult> Id(string id)
    {
        var currentUser = await userManager.GetUserAsync(User);

        return Content(currentUser.Id);
    }

    [HttpGet]
    public async Task<IActionResult> Followers()
    {
        var currentUser = await userManager.GetUserAsync(User);
        var followers = context.Users.Where(user => currentUser.FollowersId.Contains(user.Id)).ToList();
        return View(model: followers ?? new List<User>());
    }

    [HttpGet]
    public async Task<IActionResult> Followed()
    {
        var currentUser = await userManager.GetUserAsync(User);
        var followeds = context.Users.Where(user => currentUser.FollowedId.Contains(user.Id)).ToList();
        return View(model: followeds ?? new List<User>());
    }

    [HttpPost]
    public async Task MembershipAction ([FromForm] string userToActionId)
    {
        var currentUserId = (await userManager.GetUserAsync(User))?.Id;
        var followActionCommand = new FollowActionCommand { 
#pragma warning disable CS8601 // Possible null reference assignment.
            currentUserId = currentUserId ,
#pragma warning restore CS8601 // Possible null reference assignment.
            userToActionId = userToActionId
        };
        await sender.Send(followActionCommand);
    }
}