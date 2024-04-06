namespace TZTDate.Core.Data.DateUser;

public class Address
{
    public int Id { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public int PostCode { get; set; }
    public string? Street { get; set; }
    public string? District { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }

}
