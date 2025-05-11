namespace CrunchyCom.Data.Models;

public class PostSummaryDto
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Author { get; set; } = null!;
    public DateTime PublishedDate { get; set; }
    public IEnumerable<string> Tags { get; set; } = null!;
}