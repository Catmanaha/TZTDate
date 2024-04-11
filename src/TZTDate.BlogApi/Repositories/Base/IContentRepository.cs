using TZTDate.BlogApi.Models;

namespace TZTDate.BlogApi.Repositories.Base;

public interface IContentRepository
{
    public Task DeleteRangeAsync(IEnumerable<Content> contents);
    public Task UpdateAsync(Content content);
}   
