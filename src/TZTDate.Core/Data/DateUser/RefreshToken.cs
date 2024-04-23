namespace TZTDate.Core.Data.DateUser;

public class RefreshToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Guid Token { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedByIp { get; set; }
    public bool Revoked { get; set; }
    public string RevokedByIp { get; set; }
    public int? ReplacedByTokenId { get; set; }
}

