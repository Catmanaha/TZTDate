using Microsoft.EntityFrameworkCore;
using TZTDate.BlogApi.Data;
using TZTDate.BlogApi.Models;
using TZTDate.BlogApi.Repositories.Base;

namespace TZTDate.BlogApi.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly BlogDbContext blogDbContext;

    public ArticleRepository(BlogDbContext blogDbContext)
    {
        this.blogDbContext = blogDbContext;
    }

    public async Task CreateAsync(Article article)
    {
        await blogDbContext.Articles.AddAsync(article);
        await blogDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Article article)
    {
        blogDbContext.Articles.Remove(article);

        await blogDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Article>?> GetAllAsync()
    {
        var articles = await blogDbContext.Articles.Include(x => x.Contents).ToListAsync();
        return articles;
    }

    public async Task<Article?> GetByIdAsync(int id)
    {
        var article = await blogDbContext.Articles.Include(x => x.Contents).FirstOrDefaultAsync(x => x.Id == id);
        return article;
    }

    public async Task UpdateAsync(Article article)
    {
        blogDbContext.Articles.Update(article);

        await blogDbContext.SaveChangesAsync();
    }
}
