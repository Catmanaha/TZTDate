namespace TZTDate.Infrastructure.Data.DependencyInjections;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;

public static class ResponseCompressionExtension
{
    public static void InitResponse(this IServiceCollection serviceCollecyion)
    {
        serviceCollecyion.AddResponseCompression(opts => { 
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes
                .Concat(new[] { "application/octet-stream" }); 
            });
    }
}
