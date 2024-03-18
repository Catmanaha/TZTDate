using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TZTDate.Core.Data.LoveCalculator.Repositories;
using TZTDate.Infrastructure.Data.LoveCalculator.Repositories;

namespace TZTDate.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static void Inject(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ILoveCalculatorRepository, LoveCalculatorApiRepository>();
        serviceCollection.AddSingleton<HttpClient>();
        serviceCollection.AddMediatR(configurations =>
                {
                    configurations.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                });
    }
}
