using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TZTDate.WebApi.Options;
using TZTDate.Infrastructure.Data.DependencyInjections;
using TZTDate.Infrastructure.Extensions;
using TZTDate.WebApi.Middlewares;
using TZTDate.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using TZTDate.Core.Data.FaceDetectionApi.Managers;
using TZTDate.Core.Data.DateApi.Managers;

var builder = WebApplication.CreateBuilder(args);

var jwtOptionsSection = builder.Configuration.GetSection("JwtOptions");

var jwtOptions = jwtOptionsSection.Get<JwtOptions>() ??
                 throw new Exception("Couldn't create jwt options object");

builder.Services.Configure<JwtOptions>(jwtOptionsSection);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.InitMediatR();
builder.Services.InitResponse();
builder.Services.InitSignalR();
builder.Services.InitDbContext(builder.Configuration,
                               Assembly.GetExecutingAssembly());
builder.Services.Inject();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddScoped<ValidationFilterAttribute>();

builder.Services.Configure<ApiManager>(
    builder.Configuration.GetSection("ApiManager"));
builder.Services.Configure<FaceDetectionApiManager>(
    builder.Configuration.GetSection("FaceDetectionApiManager"));

builder.Services.Configure<ApiBehaviorOptions>(
    options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddSwaggerGen(options => {
  const string scheme = "Bearer";
  options.SwaggerDoc(
      "v1", new OpenApiInfo { Title = "My Identity Service", Version = "v1" });

  options.AddSecurityDefinition(name: scheme, new OpenApiSecurityScheme() {
    Description = "Enter here jwt token with Bearer",
    In = ParameterLocation.Header, Name = "Authorization",
    Type = SecuritySchemeType.Http, Scheme = scheme
  });

  options.AddSecurityRequirement(new OpenApiSecurityRequirement() {
    { new OpenApiSecurityScheme() {
       Reference =
           new OpenApiReference() { Id = scheme,
                                    Type = ReferenceType.SecurityScheme }
     },
      new string[] {} }
  });
});

builder.Services
    .AddAuthentication(o => {
      o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options => {
      options.TokenValidationParameters = new TokenValidationParameters() {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(jwtOptions.KeyInBytes),

        ValidateLifetime = true,

        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,

        ValidateIssuer = true,
        ValidIssuers = jwtOptions.Issuers,
      };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();