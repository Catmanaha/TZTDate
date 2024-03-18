using TZTDate.Infrastructure.Data.DependencyInjections;
using TZTDate.Infrastructure.Data.Hubs;
using System.Reflection;
using System.Security.Claims;
using TZTDate.Core.Data.DateUser.Enums;
using TZTDate.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admins", p =>
    {
        p.RequireRole(ClaimTypes.Role, UserRoles.Admin.ToString());
    });
});

builder.Services.AddControllersWithViews();

builder.Services.Inject();
builder.Services.InitMediatR();
builder.Services.InitSignalR();
builder.Services.InitResponse();
builder.Services.InitDbContext(builder.Configuration, Assembly.GetExecutingAssembly());

var app = builder.Build();

app.UseResponseCompression();
app.MapHub<ChatHub>("/chathub");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
