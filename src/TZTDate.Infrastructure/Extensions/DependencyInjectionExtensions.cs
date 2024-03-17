using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TZTDate.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static void Inject(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(configurations =>
                {
                    configurations.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                });
    }
}
