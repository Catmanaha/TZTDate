using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TZTDate.BlogApi.Data;

namespace TZTDate.BlogApi.Extensions;

public static class DbContextExtensions
{
    public static void InitDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<BlogDbContext>(options =>
        {
            string connectionStringKey = "DefaultConnectionString";
            string? connectionString = configuration.GetConnectionString(connectionStringKey);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new NullReferenceException($"No connection string found in appsettings.json with a key '{connectionStringKey}'");
            }

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            options.UseNpgsql(connectionString, o =>
                {
                    o.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);

                });
        });

        serviceCollection.ConfigureApplicationCookie(o =>
        {
            o.AccessDeniedPath = "/User/AccessDenied";
            o.LoginPath = "/User/Login";
        });

    }
}
