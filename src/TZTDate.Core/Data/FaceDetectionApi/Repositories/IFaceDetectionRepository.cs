namespace TZTDate.Core.Data.FaceDetectionApi.Repositories;

public interface IFaceDetectionRepository
{
    Task<bool> Detect(string filePath);
}
