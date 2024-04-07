using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TZTDate.BlogApi.Data;
using TZTDate.BlogApi.Dtos;
using TZTDate.BlogApi.Models;

namespace TZTDate.BlogApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]/[action]")]
public class ArticleController : ControllerBase
{
    private readonly BlogDbContext blogDbContext;

    public ArticleController(BlogDbContext blogDbContext)
    {
        this.blogDbContext = blogDbContext;
    }

    [HttpGet]
    public async Task<ActionResult<Article>> GetById(int articleId)
    {
        var article = await blogDbContext.Articles.Include(x => x.Contents).FirstOrDefaultAsync(x => x.Id == articleId);

        if (article is null)
        {
            return BadRequest($"No article found with {articleId} id");
        }

        return Ok(article);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int articleId)
    {
        var article = await blogDbContext.Articles.Include(x => x.Contents).FirstOrDefaultAsync(x => x.Id == articleId);

        if (article is null)
        {
            return BadRequest($"No article found with {articleId} id");
        }

        if (article.Contents is not null)
        {
            blogDbContext.Contents.RemoveRange(article.Contents);
        }

        System.IO.File.Delete($"Assets/{article.HeadPicPath}");

        blogDbContext.Articles.Remove(article);

        await blogDbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpGet]
    public async Task<IEnumerable<Article>> GetAll()
    {
        var articles = await blogDbContext.Articles.Include(x => x.Contents).ToListAsync();
        return articles;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromForm] ArticleDto articleDto)
    {
        if (articleDto.Title is null)
        {
            return BadRequest("title cannot be null");
        }

        if (articleDto.HeadPic is null)
        {
            return BadRequest("headPic cannot be null");
        }

        if (articleDto.Contents is null)
        {
            return BadRequest("contents cannot be null");
        }

        var fileExtension = new FileInfo(articleDto.HeadPic.FileName).Extension;

        var filename = $"{Guid.NewGuid()}{fileExtension}";

        var destinationAvatarPath = $"Assets/{filename}";

        using var fileStream = System.IO.File.Create(destinationAvatarPath);
        await articleDto.HeadPic.CopyToAsync(fileStream);

        var article = new Article
        {
            Title = articleDto.Title,
            HeadPicPath = filename,
            Contents = articleDto.Contents
        };

        await blogDbContext.Articles.AddAsync(article);
        await blogDbContext.SaveChangesAsync();

        return Ok();
    }
}
