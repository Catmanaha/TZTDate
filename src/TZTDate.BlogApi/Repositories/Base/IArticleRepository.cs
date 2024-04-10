using TZTDate.BlogApi.Models;

namespace TZTDate.BlogApi.Repositories.Base;

public interface IArticleRepository
{
    public Task CreateAsync(Article article);
    public Task UpdateAsync(Article article);
    public Task DeleteAsync(Article article);
    public Task<IEnumerable<Article>?> GetAllAsync();
    public Task<Article?> GetByIdAsync(int id);
}
