using TZTDate.BlogApi.Dtos;
using TZTDate.BlogApi.Models;

namespace TZTDate.BlogApi.Services.Base;

public interface IArticleService
{
    public Task CreateAsync(ArticleDto articleDto);
    public Task UpdateAsync(Article article);
    public Task DeleteAsync(int id);
    public Task<IEnumerable<Article>> GetAllAsync();
    public Task<Article> GetByIdAsync(int id);
}
