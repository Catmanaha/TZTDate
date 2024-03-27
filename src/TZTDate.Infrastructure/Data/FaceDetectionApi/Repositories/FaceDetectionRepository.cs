using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using TZTDate.Core.Data.FaceDetectionApi.Managers;
using TZTDate.Core.Data.FaceDetectionApi.Models;
using TZTDate.Core.Data.FaceDetectionApi.Repositories;

namespace TZTDate.Infrastructure.Data.FaceDetectionApi.Repositories;

public class FaceDetectionRepository : IFaceDetectionRepository
{
    private readonly HttpClient client;
    private readonly string? apiKey;
    private readonly string? apiSecret;

    public FaceDetectionRepository(IOptions<FaceDetectionApiManager> options, HttpClient client)
    {
        this.client = client;
        client.BaseAddress = new Uri(options.Value.BaseUrl);
        apiSecret = options.Value.ApiSecret;
        apiKey = options.Value.ApiKey;

    }
    public async Task<bool> Detect(string filePath)
    {
        var multipart = new MultipartFormDataContent();
        var fileExtension = Path.GetExtension(filePath)[1..];
        var fileStream = new FileStream(filePath, FileMode.Open);
        var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue($"image/{fileExtension}");

        multipart.Add(streamContent, "image_file", Path.GetFileName(filePath));

        var response = await client.PostAsync($"?api_key={apiKey}&api_secret={apiSecret}", multipart);

        var responseContent = await response.Content.ReadFromJsonAsync<FaceDetectResponse>();
        
        if (responseContent?.FaceNum == 0) {
            return false;
        }

        return true;
    }
}
