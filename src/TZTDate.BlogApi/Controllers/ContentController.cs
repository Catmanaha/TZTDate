using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TZTDate.BlogApi.Models;
using TZTDate.BlogApi.Services.Base;

namespace TZTDate.BlogApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class ContentController : ControllerBase
{
    private readonly IContentService contentService;

    public ContentController(IContentService contentService)
    {
        this.contentService = contentService;
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Content content)
    {
        await contentService.UpdateAsync(content);

        return Ok();
    }
}
