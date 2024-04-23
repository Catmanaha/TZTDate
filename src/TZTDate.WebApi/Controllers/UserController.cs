using Azure.Storage;
using Azure.Storage.Sas;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TZTDate.Core.Data.DateUser;
using TZTDate.Core.Data.DateUser.Dtos;
using TZTDate.Core.Data.DateUser.Enums;
using TZTDate.Core.Data.FaceDetectionApi.Repositories;
using TZTDate.Core.Data.Options;
using TZTDate.Core.Data.SearchData;
using TZTDate.Infrastructure.Data;
using TZTDate.Infrastructure.Data.DateUser.Commands;
using TZTDate.Infrastructure.Data.SearchData.Services;
using TZTDate.Infrastructure.Services.Base;
using TZTDate.WebApi.Filters;

namespace TZTDate.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
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

        [HttpGet]
        public async Task<ActionResult<User>> Account(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            var userWithAddress = await context.Users.Include(x => x.Address)
                                      .FirstOrDefaultAsync(o => o.Id == user.Id);

            var ImageUris = new List<string>();

            foreach (var path in user.ProfilePicPaths)
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

        [HttpGet]
        public async Task<ActionResult<User>> Details(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(user => user.Id == id);

            var ImageUris = new List<string>();

            foreach (var path in user.ProfilePicPaths)
            {
                var securePath = azureBlobService.GetBlobItemSAS(path);

                ImageUris.Add(securePath);
            }


            return Ok(new { User = user, ImageUris });
        }

        [HttpGet]
        public async Task<IActionResult> Profiles(int userId, string? searchByName, int? startAge, int? endAge, string? interests, Gender? searchGender)
        {
            var me = await sender.Send(new FindByIdCommand
            {
                Id = userId
            });

            var users = await context.Users.ToListAsync();

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
                Users = users.GetRange(0, users.Count() < pageItemsCount ? users.Count() : pageItemsCount)
            });
        }

        [HttpGet]
        public async Task<IActionResult> LoadMoreProfiles(int userId, int skip, string? searchByName, int? startAge, int? endAge, string? interests, string? searchGender)
        {
            var me = await sender.Send(new FindByIdCommand
            {
                Id = userId
            });

            var users = await context.Users.ToListAsync();

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

            users = users.GetRange(skip, users.Count() - skip < pageItemsCount ? users.Count() - skip : pageItemsCount);

            return Ok(users);
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
        public async Task MembershipAction(int userToActionId, int currentUserId)
        {
            var followActionCommand = new FollowActionCommand
            {
                currentUserId = currentUserId,
                userToActionId = userToActionId
            };
            await sender.Send(followActionCommand);
        }
    }
}
