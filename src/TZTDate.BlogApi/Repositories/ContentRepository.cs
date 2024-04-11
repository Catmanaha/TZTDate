using TZTDate.BlogApi.Data;
using TZTDate.BlogApi.Models;
using TZTDate.BlogApi.Repositories.Base;

namespace TZTDate.BlogApi.Repositories;

public class ContentRepository : IContentRepository
{
    private readonly BlogDbContext blogDbContext;
    public ContentRepository(BlogDbContext blogDbContext)
    {
        this.blogDbContext = blogDbContext;

    }

    public async Task DeleteRangeAsync(IEnumerable<Content> contents)
    {
        blogDbContext.Contents.RemoveRange(contents);

        await blogDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Content content)
    {
        blogDbContext.Contents.Update(content);
        await blogDbContext.SaveChangesAsync();
    }
}
