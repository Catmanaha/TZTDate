using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TZTDate.Core.Data.Options;

namespace TZTDate.Infrastructure.Extensions;

public static class ConfigureExtensions
{
    public static void Configure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {

        serviceCollection.Configure<ApiOption>(o => configuration.GetSection("ApiOption"));
        serviceCollection.Configure<FaceDetectionApiOption>(o => configuration.GetSection("FaceDetectionApiOption"));
        serviceCollection.Configure<JwtOption>(o => configuration.GetSection("JwtOption"));
        serviceCollection.Configure<BlobOption>(o => configuration.GetSection("BlobOption"));
    }
}
