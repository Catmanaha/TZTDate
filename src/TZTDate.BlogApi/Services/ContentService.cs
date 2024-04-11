using TZTDate.BlogApi.Models;
using TZTDate.BlogApi.Repositories.Base;
using TZTDate.BlogApi.Services.Base;

namespace TZTDate.BlogApi.Services;

public class ContentService : IContentService
{
    private readonly IContentRepository contentRepository;

    public ContentService(IContentRepository contentRepository)
    {
        this.contentRepository = contentRepository;
    }

    public async Task DeleteRangeAsync(IEnumerable<Content> contents)
    {
        if (contents is null)
        {
            throw new BadHttpRequestException("null", new NullReferenceException($"{nameof(contents)} cannot be null"));
        }

        if (!contents.Any())
        {
            return;
        }

        await contentRepository.DeleteRangeAsync(contents);
    }

    public async Task UpdateAsync(Content content)
    {
        if (content is null)
        {
            throw new BadHttpRequestException("null", new NullReferenceException($"{nameof(content)} cannot be null"));
        }

        await contentRepository.UpdateAsync(content);
    }
}
