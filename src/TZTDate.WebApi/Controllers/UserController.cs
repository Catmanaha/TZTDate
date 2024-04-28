using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateUser;
using TZTDate.Core.Data.DateUser.Dtos;
using TZTDate.Core.Data.DateUser.Enums;
using TZTDate.Core.Data.SearchData;
using TZTDate.Infrastructure.Data;
using TZTDate.Infrastructure.Data.DateUser.Commands;
using TZTDate.Infrastructure.Data.SearchData.Services;
using TZTDate.Infrastructure.Services.Base;
using TZTDate.WebApi.Filters;

namespace TZTDate.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[ServiceFilter(typeof(ValidationFilterAttribute))]
public class UserController : ControllerBase
{
    private const int pageItemsCount = 12;
    private readonly ISender sender;
    private readonly TZTDateDbContext context;
    private readonly IAzureBlobService azureBlobService;

    public UserController(ISender sender,
                          TZTDateDbContext context,
                          IAzureBlobService azureBlobService)
    {
        this.sender = sender;
        this.context = context;
        this.azureBlobService = azureBlobService;
    }

    [HttpGet()]
    public async Task<ActionResult<User>> Account(int id)
    {
        var userWithAddress = await context.Users
            .Include(x => x.Address)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (userWithAddress == null)
        {
            return NotFound($"User with id '{id}' doesn't exist!");
        }

        var ImageUris = new List<string>();

        foreach (var path in userWithAddress.ProfilePicPaths)
        {
            var securePath = azureBlobService.GetBlobItemSAS(path);

            ImageUris.Add(securePath);
        }

        return Ok(new AccountDto
        {
            User = userWithAddress,
            ImageUris = ImageUris
        });
    }

    [HttpGet()]
    public async Task<ActionResult<User>> Details(int currentUserId, int viewedUserId)
    {
        var user = await sender.Send(new FindByIdCommand
        {
            Id = viewedUserId

        });

        if (user == null)
        {
            return NotFound($"User with id '{viewedUserId}' doesn't exist!");
        }

        var currentUser = await sender.Send(new FindByIdCommand
        {
            Id = currentUserId

        });



        if (currentUser == null)
        {
            return NotFound($"User with id '{currentUserId}' doesn't exist!");
        }

        var ImageUris = new List<string>();

        foreach (var path in user.ProfilePicPaths)
        {
            var securePath = azureBlobService.GetBlobItemSAS(path);

            ImageUris.Add(securePath);
        }

        return Ok(new { User = user, ImageUris, MyUser = currentUser });
    }



    [HttpGet()]
    public async Task<IActionResult> Profiles(int userId, string? searchByName, int? startAge, int? endAge, string? interests, Gender? searchGender)
    {
        var me = await sender.Send(new FindByIdCommand
        {
            Id = userId
        });

        if (me == null)
        {
            return NotFound($"User with id '{userId}' doesn't exist!");
        }

        var users = await context.Users.ToListAsync();

        users.ForEach(user =>
        {
            for (int i = 0; i < user?.ProfilePicPaths.Count(); i++)
            {
                user.ProfilePicPaths[i] = azureBlobService.GetBlobItemSAS(user.ProfilePicPaths[i]);
            }
        });

        SearchData searchData = new SearchData()
        {
            Me = me,
            Users = users,
            SearchingGender = searchGender,
            SearchingStartAge = startAge,
            SearchingEndAge = endAge,
            SearchingInterests = interests,
            SearchingUsername = searchByName
        };

        users = SearchDataService.ProfilesFilter(searchData);

        return Ok(new
        {
            SearchingStartAge = me.SearchingAgeStart,
            SearchingEndAge = me.SearchingAgeEnd,
            SearchingGender = me.SearchingGender.ToString(),
            Users = users
        });

        // return Ok(new
        // {
        //     SearchingStartAge = me.SearchingAgeStart,
        //     SearchingEndAge = me.SearchingAgeEnd,
        //     SearchingGender = me.SearchingGender.ToString(),
        //     Users = users.Take(pageItemsCount)
        // });
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsersAsync()
    {
        return Ok(await context?.Users?.ToListAsync());
    }


    [HttpGet]
    public async Task<IActionResult> LoadMoreProfiles(int userId, int skip, string? searchByName, int? startAge, int? endAge, string? interests, string? searchGender)
    {
        var me = await sender.Send(new FindByIdCommand
        {
            Id = userId
        });

        if (me == null)
        {
            return NotFound($"User with id '{userId}' doesn't exist!");
        }

        var users = context.Users.AsQueryable();

        users = SearchDataService.MoreProfilesFilter(new SearchData()
        {
            Me = me,
            Users = users,
            SearchingGender = searchGender == "0" ? Gender.Male : Gender.Female,
            SearchingStartAge = startAge,
            SearchingEndAge = endAge,
            SearchingInterests = interests,
            SearchingUsername = searchByName
        });

        var usersList = users.ToList();

        if (skip < usersList.Count)
        {
            usersList = usersList.Skip(skip).Take(pageItemsCount).ToList();
        }
        else
        {
            return NotFound("No more users to load.");
        }

        return Ok(usersList);
    }

    [HttpGet]
    public async Task<IActionResult> Followers(int userId)
    {
        var currentUser = await sender.Send(new FindByIdCommand
        {
            Id = userId
        });

        var followers = context.UserFollows
            .Where(uf => uf.FollowedId == userId)
            .Select(uf => uf.Follower)
            .ToList();

        return Ok(followers ?? new List<User>());
    }

    [HttpGet]
    public async Task<IActionResult> Followed(int userId)
    {
        var currentUser = await sender.Send(new FindByIdCommand
        {
            Id = userId
        });

        var followedUsers = context.UserFollows
            .Where(uf => uf.FollowerId == userId)
            .Select(uf => uf.Followed)
            .ToList();

        return Ok(followedUsers ?? new List<User>());
    }

    [HttpPost]
    public async Task<IActionResult> MembershipAction(int userToActionId, int currentUserId)
    {
        var followActionCommand = new FollowActionCommand
        {
            currentUserId = currentUserId,
            userToActionId = userToActionId
        };
        await sender.Send(followActionCommand);

        return Ok();
    }
}
