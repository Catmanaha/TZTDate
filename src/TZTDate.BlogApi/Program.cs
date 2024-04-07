using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TZTDate.BlogApi.Extensions;
using TZTDate.BlogApi.Models.Managers;
using TZTDate.BlogApi.Services;
using TZTDate.BlogApi.Services.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var jwtManagerSection = builder.Configuration
    .GetSection("JwtManager");

var jwtManager = jwtManagerSection.Get<JwtManager>() ?? throw new Exception("Couldn't create jwt manager object");

builder.Services.Configure<JwtManager>(jwtManagerSection);

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
        new OpenApiSecurityScheme()
        {
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

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.InitDbContext(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtManager.KeyInBytes),

            ValidateLifetime = true,

            ValidateAudience = true,
            ValidAudience = jwtManager.Audience,

            ValidateIssuer = true,
            ValidIssuer = jwtManager.Issuer,
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
