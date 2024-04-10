using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TZTDate.Infrastructure.Data;
using TZTDate.Core.Data.DateUser;

namespace TZTDate.Infrastructure.Extensions;

public static class DbContextExtensions
{
    public static void InitDbContext(this IServiceCollection serviceCollection, IConfiguration configuration, Assembly assembly)
    {
        serviceCollection.AddDbContext<TZTDateDbContext>(options =>
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
                    o.MigrationsAssembly(assembly.FullName);
                    
                });
        });

        serviceCollection.AddIdentity<User, IdentityRole>()
                         .AddEntityFrameworkStores<TZTDateDbContext>();

        serviceCollection.ConfigureApplicationCookie(o =>
        {
            o.AccessDeniedPath = "/User/AccessDenied";
            o.LoginPath = "/User/Login";
        });

    }
}
