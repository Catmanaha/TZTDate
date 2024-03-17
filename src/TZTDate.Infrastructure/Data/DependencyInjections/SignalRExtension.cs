namespace TZTDate.Infrastructure.Data.DependencyInjections;

using Microsoft.Extensions.DependencyInjection;
public static class SignalRExtension
{
    public static void InitSignalR(this IServiceCollection serviceCollecyion)
    {
        serviceCollecyion.AddSignalR();
    }
}
