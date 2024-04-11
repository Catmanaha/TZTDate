using TZTDate.BlogApi.Models;

namespace TZTDate.BlogApi.Services.Base;

public interface IContentService
{
    public Task DeleteRangeAsync(IEnumerable<Content> contents);
    public Task UpdateAsync(Content content);
}
