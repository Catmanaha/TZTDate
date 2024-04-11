namespace TZTDate.BlogApi.Models;

public class Article
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? HeadPicPath { get; set; }
    public IEnumerable<Content>? Contents { get; set; }
}
