using TZTDate.BlogApi.Dtos;
using TZTDate.BlogApi.Models;
using TZTDate.BlogApi.Repositories.Base;
using TZTDate.BlogApi.Services.Base;

namespace TZTDate.BlogApi.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository articleRepository;
    private readonly IContentRepository contentRepository;

    public ArticleService(IArticleRepository articleRepository, IContentRepository contentRepository)
    {
        this.contentRepository = contentRepository;
        this.articleRepository = articleRepository;
    }

    public async Task CreateAsync(ArticleDto articleDto)
    {
        var errors = new List<BadHttpRequestException>();

        if (articleDto.Title is null)
        {
            errors.Add(new BadHttpRequestException("Argument null", new NullReferenceException($"{nameof(articleDto.Title)} cannot be null")));
        }

        if (articleDto.HeadPic is null)
        {
            errors.Add(new BadHttpRequestException("Argument null", new NullReferenceException($"{nameof(articleDto.HeadPic)} cannot be null")));
        }

        if (articleDto.Contents is null)
        {
            errors.Add(new BadHttpRequestException("Argument null", new NullReferenceException($"{nameof(articleDto.Contents)} cannot be null")));
        }

        if (errors.Any())
        {
            throw new AggregateException(errors);
        }

        var fileExtension = new FileInfo(articleDto.HeadPic.FileName).Extension;

        var filename = $"{Guid.NewGuid()}{fileExtension}";

        var destinationAvatarPath = $"Assets/{filename}";

        using var fileStream = File.Create(destinationAvatarPath);
        await articleDto.HeadPic.CopyToAsync(fileStream);

        var article = new Article
        {
            Title = articleDto.Title,
            HeadPicPath = filename,
            Contents = articleDto.Contents
        };

        await articleRepository.CreateAsync(article);
    }

    public async Task DeleteAsync(int id)
    {
        if (id < 0)
        {
            throw new BadHttpRequestException($"{nameof(id)} cannot be negative");
        }

        var article = await GetByIdAsync(id);

        if (article!.Contents is not null)
        {
            await contentRepository.DeleteRangeAsync(article.Contents);
        }

        File.Delete($"Assets/{article.HeadPicPath}");

        await articleRepository.DeleteAsync(article);
    }

    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        var articles = await articleRepository.GetAllAsync();

        return articles ?? new List<Article>();
    }

    public async Task<Article> GetByIdAsync(int id)
    {
        if (id < 0)
        {
            throw new BadHttpRequestException($"{nameof(id)} cannot be negative");
        }

        var article = await articleRepository.GetByIdAsync(id);

        if (article is null)
        {
            throw new BadHttpRequestException($"Article not found with {id} id");
        }

        return article;
    }

    public async Task UpdateAsync(Article article)
    {
        if (article is null)
        {
            throw new BadHttpRequestException("Argument null", new NullReferenceException($"{nameof(article)} cannot be null"));
        }

        await articleRepository.UpdateAsync(article);
    }
}
