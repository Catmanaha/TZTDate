using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateUser;
using TZTDate.Core.Data.DateUserAndRecomendations;
using TZTDate.Infrastructure.Data;
using TZTDate.Presentation.Models;

namespace TZTDate.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly TZTDateDbContext context;
    private readonly UserManager<User> userManager;

    public HomeController(TZTDateDbContext context, UserManager<User> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        User me = await userManager.GetUserAsync(User);
        IEnumerable<User> users = await context.Users.ToListAsync();

        users = users.Where(u => u.Id != me.Id).ToList();
        users = users.Where(u => u.Age >= me.SearchingAgeStart).ToList();
        users = users.Where(u => u.Age <= me.SearchingAgeEnd).ToList();
        users = users.Where(u => u.Gender == me.SearchingGender).ToList();
        users = users.Where(u => u.Address.Country == me.Address.Country);
        users = users.Where(u => u.Address.City == me.Address.City);
        string[] interestsArray = me.Interests.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        users = users.Where(u => u.Interests != null && u.Interests.Split(' ', StringSplitOptions.RemoveEmptyEntries).Intersect(interestsArray).Any()).ToList();

        DateUserAndRecomendations meAndRecomendations = new DateUserAndRecomendations();
        meAndRecomendations.Me = me;
        meAndRecomendations.RecomendationUsers = users.Take(5);

        return View(meAndRecomendations);
        
    }

    public IActionResult Main()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
