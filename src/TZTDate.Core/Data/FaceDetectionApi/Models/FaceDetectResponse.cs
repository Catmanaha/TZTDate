using System.Text.Json.Serialization;

namespace TZTDate.Core.Data.FaceDetectionApi.Models;

public class FaceDetectResponse
{
    [JsonPropertyName("face_num")]
    public int FaceNum { get; set; }
}
