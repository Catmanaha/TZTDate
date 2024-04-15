using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TZTBank.Infrastructure.Data.BankUser.Handlers;
using TZTBank.Infrastructure.Data.DateUser.Commands;
using TZTDate.Core.Data.DateUser;
using TZTDate.WebApi.Options;
using TZTDate.Infrastructure.Data.DependencyInjections;
using TZTDate.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var jwtOptionsSection = builder.Configuration
    .GetSection("JwtOptions");

var jwtOptions = jwtOptionsSection.Get<JwtOptions>() ?? throw new Exception("Couldn't create jwt options object");

builder.Services.Configure<JwtOptions>(jwtOptionsSection);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.InitMediatR();
builder.Services.InitResponse();
builder.Services.InitSignalR();
builder.Services.InitDbContext(builder.Configuration, Assembly.GetExecutingAssembly());
builder.Services.Inject();

builder.Services.AddScoped<IRequestHandler<LoginCommand>, LoginHandler>();

builder.Services.AddSwaggerGen(options =>
{
    const string scheme = "Bearer";
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My Identity Service",
        Version = "v1"
    });

    options.AddSecurityDefinition(
        name: scheme,
        new OpenApiSecurityScheme() {
            Description = "Enter here jwt token with Bearer",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = scheme
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement() {
            {
                new OpenApiSecurityScheme() {
                    Reference = new OpenApiReference() {
                        Id = scheme,
                        Type = ReferenceType.SecurityScheme
                    }
                } ,
                new string[] {}
            }
        }
    );
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();