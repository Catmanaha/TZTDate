using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TZTDate.Core.Data.Options;

namespace TZTDate.Infrastructure.Extensions;

public static class ConfigureExtensions
{
    public static void Configure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {

        serviceCollection.Configure<ApiOption>(configuration.GetSection("ApiOption"));
        serviceCollection.Configure<FaceDetectionApiOption>(configuration.GetSection("FaceDetectionApiOption"));
        serviceCollection.Configure<JwtOption>(configuration.GetSection("JwtOption"));
        serviceCollection.Configure<BlobOption>(configuration.GetSection("BlobOption"));
    }
}
