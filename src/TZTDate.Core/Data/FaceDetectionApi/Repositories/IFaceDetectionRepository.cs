using Microsoft.AspNetCore.Http;

namespace TZTDate.Core.Data.FaceDetectionApi.Repositories;

public interface IFaceDetectionRepository
{
    Task<bool> Detect(IFormFile filePath);
}
