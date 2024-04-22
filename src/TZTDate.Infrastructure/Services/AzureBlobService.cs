using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TZTDate.Core.Data.Options;
using TZTDate.Infrastructure.Services.Base;

namespace TZTDate.Infrastructure.Services;

public class AzureBlobService : IAzureBlobService
{
    BlobServiceClient blobServiceClient;
    BlobContainerClient blobContainerClient;

    public AzureBlobService(IOptionsSnapshot<BlobOptions> optionsSnapshot)
    {
        blobServiceClient = new BlobServiceClient(optionsSnapshot.Value.ConnectionString);
        blobContainerClient = blobServiceClient.GetBlobContainerClient(optionsSnapshot.Value.ContainerName);
    }

    public async Task<List<BlobContentInfo>> UploadFiles(List<IFormFile> files)
    {
        var azureResponse = new List<BlobContentInfo>();

        foreach (var file in files)
        {
            string fileName = file.FileName;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                memoryStream.Position = 0;

                var client = await blobContainerClient.UploadBlobAsync(fileName, memoryStream);
                azureResponse.Add(client);
            }
        }

        return azureResponse;
    }

    public async Task Uploadfile(IFormFile file, string fileName = null)
    {
        string fileNameResult = fileName is null ? file.FileName : fileName;
        using (var memoryStream = new MemoryStream())
        {
            file.CopyTo(memoryStream);
            memoryStream.Position = 0;

            await blobContainerClient.UploadBlobAsync(fileNameResult, memoryStream);
        }
    }

    public async Task<List<BlobItem>> GetBlob()
    {
        var items = new List<BlobItem>();

        var uploadedFiles = blobContainerClient.GetBlobsAsync();

        await foreach (BlobItem file in uploadedFiles)
        {
            items.Add(file);
        }

        return items;
    }

    public async Task DeleteBlob(string blobName)
    {
        await blobContainerClient.DeleteBlobAsync(blobName);
    }

    public BlobClient GetBlobItemAsync(string blobName)
    {
        return blobContainerClient.GetBlobClient(blobName);
    }
}
