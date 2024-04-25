using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TZTDate.Core.Data.FaceDetectionApi.Repositories;
using TZTDate.Core.Data.LoveCalculator.Repositories;
using TZTDate.Infrastructure.Data.FaceDetectionApi.Repositories;
using TZTDate.Infrastructure.Data.LoveCalculator.Repositories;
using TZTDate.Infrastructure.Services;
using TZTDate.Infrastructure.Services.Base;

namespace TZTDate.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static void Inject(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ILoveCalculatorRepository, LoveCalculatorApiRepository>();
        serviceCollection.AddSingleton<IFaceDetectionRepository, FaceDetectionRepository>();
        serviceCollection.AddScoped<IAzureBlobService, AzureBlobService>();
        serviceCollection.AddSingleton<HttpClient>();
    }
}
