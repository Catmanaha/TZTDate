using System.Text;

namespace TZTDate.BlogApi.Models.Managers;

public class JwtManager
{
    public string? Key { get; set; }
    public byte[]? KeyInBytes => Encoding.UTF8.GetBytes(Key);
    public string? Audience { get; set; }
    public int LifetimeInMinutes { get; set; }
    public string? Issuer { get; set; }
}
