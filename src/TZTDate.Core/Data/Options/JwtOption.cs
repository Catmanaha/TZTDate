using System.Text;

namespace TZTDate.Core.Data.Options;

public class JwtOption
{
    public string Key { get; set; }
    public byte[] KeyInBytes => Encoding.UTF8.GetBytes(this.Key);
    public string Audience { get; set; }
    public int LifetimeInMinutes { get; set; }
    public IEnumerable<string> Issuers { get; set; }
    public int RefreshTokenLifetimeInHours { get; set; }
}