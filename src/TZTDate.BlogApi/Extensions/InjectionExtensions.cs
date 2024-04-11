using TZTDate.BlogApi.Data;
using TZTDate.BlogApi.Middlewares;
using TZTDate.BlogApi.Repositories;
using TZTDate.BlogApi.Repositories.Base;
using TZTDate.BlogApi.Services;
using TZTDate.BlogApi.Services.Base;

namespace TZTDate.BlogApi.Extensions;

public static class InjectionExtensions
{
    public static void Inject(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ITokenService, TokenService>();
        serviceCollection.AddScoped<IArticleService, ArticleService>();
        serviceCollection.AddScoped<IContentService, ContentService>();

        serviceCollection.AddScoped<IArticleRepository, ArticleRepository>();
        serviceCollection.AddScoped<IContentRepository, ContentRepository>();

        serviceCollection.AddTransient<ExceptionHandlingMiddleware>();

        serviceCollection.AddScoped<BlogDbContext>();
    }
}
