using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace TZTDate.Infrastructure.Services.Base;

public interface IAzureBlobService
{
    public Task<List<BlobContentInfo>> UploadFiles(List<IFormFile> files);
    public Task UploadFile(Stream file, string fileName);
    public Task<List<BlobItem>> GetBlob();
    public Task DeleteBlob(string blobName);
    public BlobClient GetBlobItem(string blobName);
    public string GetBlobItemSAS(string path);
}
