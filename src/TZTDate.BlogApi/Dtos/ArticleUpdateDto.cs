using TZTDate.BlogApi.Models;

namespace TZTDate.BlogApi.Dtos;

public class ArticleUpdateDto
{
    public int Id { get; set;}
    public string? Title { get; set; }
    public IFormFile? HeadPic { get; set; }
    public List<Content> Contents { get; set; }
}
