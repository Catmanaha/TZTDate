using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TZTDate.BlogApi.Dtos;
using TZTDate.BlogApi.Models;
using TZTDate.BlogApi.Services.Base;

namespace TZTDate.BlogApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]/[action]")]
public class ArticleController : ControllerBase
{
    private readonly IArticleService articleService;

    public ArticleController(IArticleService articleService)
    {
        this.articleService = articleService;
    }

    [HttpGet]
    public async Task<ActionResult<Article>> GetById(int articleId)
    {
        return Ok(await articleService.GetByIdAsync(articleId));
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int articleId)
    {
        await articleService.DeleteAsync(articleId);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Article>>> GetAll()
    {
        var articles = await articleService.GetAllAsync();

        return Ok(articles);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromForm] ArticleCreateDto articleDto)
    {
        await articleService.CreateAsync(articleDto);

        return Ok();
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromForm] ArticleUpdateDto articleDto)
    {
        await articleService.UpdateAsync(articleDto);

        return Ok();
    }
}
