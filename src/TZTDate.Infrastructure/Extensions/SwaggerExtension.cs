using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace TZTDate.Infrastructure.Extensions;

public static class SwaggerExtension
{
    public static void InitSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(options =>
        {
            const string scheme = "Bearer";
            options.SwaggerDoc(
                "v1", new OpenApiInfo { Title = "My Identity Service", Version = "v1" });

            options.AddSecurityDefinition(name: scheme, new OpenApiSecurityScheme()
            {
                Description = "Enter here jwt token with Bearer",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = scheme
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
    }
}
