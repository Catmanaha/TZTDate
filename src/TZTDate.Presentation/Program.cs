using TZTDate.Infrastructure.Data.DependencyInjections;
using TZTDate.Infrastructure.Data.Hubs;
using System.Reflection;
using System.Security.Claims;
using TZTDate.Core.Data.DateUser.Enums;
using TZTDate.Infrastructure.Extensions;
using TZTDate.Core.Data.DateApi.Managers;
using TZTDate.Core.Data.FaceDetectionApi.Managers;
using TZTDate.Presentation.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admins", p =>
    {
        p.RequireRole(ClaimTypes.Role, UserRoles.Admin.ToString());
    });
});

builder.Services.Configure<ApiManager>(builder.Configuration.GetSection("ApiManager"));
builder.Services.Configure<FaceDetectionApiManager>(builder.Configuration.GetSection("FaceDetectionApiManager"));
builder.Services.AddControllersWithViews();

builder.Services.Inject();
builder.Services.InitMediatR();
builder.Services.InitSignalR();
builder.Services.InitResponse();
builder.Services.InitDbContext(builder.Configuration, Assembly.GetExecutingAssembly());

builder.Services.AddScoped<RelationshipCalcComponent>();

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
