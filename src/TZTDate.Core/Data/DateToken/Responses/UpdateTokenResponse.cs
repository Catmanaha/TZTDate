namespace TZTDate.Core.Data.DateToken.Responses;

public class UpdateTokenResponse
{
    public string AccessToken { get; set; }
    public Guid RefreshToken { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
}
