using System.Reflection;
using TZTDate.Infrastructure.Extensions;
using TZTDate.Infrastructure.Data.ChatHub;
using TZTDate.WebApi.Middlewares;
using TZTDate.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using TZTDate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateUser;
using TZTDate.Core.Data.DateUser.Enums;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddResponseCompression(opts =>
{
   opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
         new[] { "application/octet-stream" });
});
builder.Services.InitResponse();
builder.Services.Inject();
builder.Services.InitSwagger();
builder.Services.InitAuthentication(builder.Configuration);
builder.Services.Configure(builder.Configuration);
builder.Services.InitDbContext(builder.Configuration, Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(configurations => configurations.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("TZTDate.Infrastructure")));

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddScoped<ValidationFilterAttribute>();

builder.Services.AddSignalR();

builder.Services.Configure<ApiBehaviorOptions>(
    options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
  options.AddPolicy("BlazorWasmPolicy", corsBuilder =>
  {
    corsBuilder
        .WithOrigins("http://localhost:5251")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
  });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var context = scope.ServiceProvider.GetRequiredService<TZTDateDbContext>();
  await context.Database.MigrateAsync();
  await context.Database.EnsureCreatedAsync();

  var userRoleExists = await context.Roles.AnyAsync(r => r.Name == "User");
  var adminRoleExists = await context.Roles.AnyAsync(r => r.Name == "Admin");

  if (!userRoleExists)
  {
    var userRole = new Role { Name = UserRoles.User.ToString() };
    await context.Roles.AddAsync(userRole);
  }

  if (!adminRoleExists)
  {
    var adminRole = new Role { Name = UserRoles.Admin.ToString() };
    await context.Roles.AddAsync(adminRole);
  }

  await context.SaveChangesAsync();
}

app.MapHub<ChatHub>("/chat");
app.UseSwagger();
app.UseSwaggerUI();
app.UseResponseCompression();
app.UseCors("BlazorWasmPolicy");
app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
