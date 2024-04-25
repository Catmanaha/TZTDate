using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TZTDate.Core.Data.FaceDetectionApi.Models;
using TZTDate.Core.Data.FaceDetectionApi.Repositories;
using TZTDate.Core.Data.Options;

namespace TZTDate.Infrastructure.Data.FaceDetectionApi.Repositories;

public class FaceDetectionRepository : IFaceDetectionRepository
{
    private readonly HttpClient client;
    private readonly string? apiKey;
    private readonly string? apiSecret;

    public FaceDetectionRepository(IOptions<FaceDetectionApiOption> options, HttpClient client)
    {
        this.client = client;
        client.BaseAddress = new Uri(options.Value.BaseUrl);
        apiSecret = options.Value.ApiSecret;
        apiKey = options.Value.ApiKey;

    }
    public async Task<bool> Detect(IFormFile file)
    {
        var fileExtensionFormFile = new FileInfo(file.FileName).Extension;

        var filename = $"Temp{Guid.NewGuid()}{fileExtensionFormFile}";

        var destinationAvatarPath = $"wwwroot/Assets/{filename}";

        using var fileStreamCreate = System.IO.File.Create(destinationAvatarPath);
        await file.CopyToAsync(fileStreamCreate);
        fileStreamCreate.Close();

        var multipart = new MultipartFormDataContent();
        var fileExtension = Path.GetExtension(destinationAvatarPath)[1..];
        var fileStream = new FileStream(destinationAvatarPath, FileMode.Open);
        var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue($"image/{fileExtension}");

        multipart.Add(streamContent, "image_file", Path.GetFileName(destinationAvatarPath));

        var response = await client.PostAsync($"?api_key={apiKey}&api_secret={apiSecret}", multipart);

        var responseContent = await response.Content.ReadFromJsonAsync<FaceDetectResponse>();

        fileStream.Close();

        if (responseContent?.FaceNum == 0)
        {
            File.Delete(destinationAvatarPath);

            return false;
        }

        File.Delete(destinationAvatarPath);

        return true;
    }
}
