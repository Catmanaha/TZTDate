using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateUser;
using TZTDate.Core.Data.DateUserAndRecomendations;
using TZTDate.Infrastructure.Data;
using TZTDate.Infrastructure.Data.DateUser.Commands;
using TZTDate.Infrastructure.Services.Base;
using TZTDate.WebApi.Filters;

namespace TZTDate.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[ServiceFilter(typeof(ValidationFilterAttribute))]
public class HomeController : ControllerBase
{
    private readonly ISender sender;
    private readonly TZTDateDbContext context;
    private readonly IAzureBlobService azureBlobService;

    public HomeController(ISender sender, TZTDateDbContext context, IAzureBlobService azureBlobService)
    {
        this.azureBlobService = azureBlobService;
        this.sender = sender;
        this.context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index(int userId)
    {
        var me = await sender.Send(new FindByIdCommand { Id = userId });

        IEnumerable<User> users = await context.Users.ToListAsync();

        users = users.Where(u => u.Id != me.Id
            && u.Age >= me.SearchingAgeStart
            && u.Age <= me.SearchingAgeEnd
            && u.Gender == me.SearchingGender
            && u.Address?.Country == me.Address?.Country
            && u.Address?.City == me.Address?.City).ToList();

        string[] interestsArray = me.Interests.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        users = users.Where(u => u.Interests != null && u.Interests.Split(' ', StringSplitOptions.RemoveEmptyEntries).Intersect(interestsArray).Any()).ToList();

        foreach (var user in users)
        {
            user.ProfilePicPaths = user.ProfilePicPaths.Select(p => azureBlobService.GetBlobItemSAS(p)).ToArray();
        }

        DateUserAndRecomendations meAndRecomendations = new DateUserAndRecomendations
        {
            Me = me,
            RecomendationUsers = users.Take(5)
        };

        return Ok(meAndRecomendations);

    }
}
