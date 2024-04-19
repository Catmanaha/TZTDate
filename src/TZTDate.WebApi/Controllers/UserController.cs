using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateUser;
using TZTDate.Core.Data.FaceDetectionApi.Repositories;
using TZTDate.Infrastructure.Data;
using TZTDate.WebApi.Filters;

namespace TZTDate.WebApi.Controllers {
[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
[ServiceFilter(typeof(ValidationFilterAttribute))]
public class UserController : ControllerBase {
  private const int pageItemsCount = 12;
  private readonly ISender sender;
  private readonly SignInManager<User> signInManager;
  private readonly UserManager<User> userManager;
  private readonly TZTDateDbContext context;
  private readonly IFaceDetectionRepository faceDetectionRepository;

  public UserController(ISender sender, SignInManager<User> signInManager,
                        UserManager<User> userManager, TZTDateDbContext context,
                        IFaceDetectionRepository faceDetectionRepository) {
    this.sender = sender;
    this.signInManager = signInManager;
    this.userManager = userManager;
    this.context = context;
    this.faceDetectionRepository = faceDetectionRepository;
  }

  [HttpGet]
  public async Task<ActionResult<User>> Account() {
    User user = await userManager.GetUserAsync(User);
    var userWithAddress = await userManager.Users.Include(x => x.Address)
                              .FirstOrDefaultAsync(o => o.Id == user.Id);

    return Ok(userWithAddress);
  }

  [HttpGet]
  public async Task<ActionResult<User>> Details(string id) {
    var user = await context.Users.FirstOrDefaultAsync(user => user.Id == id);

    return Ok(user);
  }
}
}
