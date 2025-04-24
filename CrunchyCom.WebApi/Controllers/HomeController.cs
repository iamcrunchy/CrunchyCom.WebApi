using Microsoft.AspNetCore.Mvc;

namespace CrunchyCom.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    // GET
    public IActionResult Index()
    {
        return Ok();
    }
    
    // GET: /home/article
    [HttpGet("article")]
    public IActionResult GetArticle()
    {
        var date = DateTime.UtcNow;
        var usCulture = new System.Globalization.CultureInfo("en-US");
        var dateString = date.ToString("MMMM dd, yyyy", usCulture);

        try
        {
            var mdPath = "E:\\Mobile Forensics Development\\Module 1 - Introduction to Mobile Forensics.md";
            var markdownContent = System.IO.File.ReadAllText(mdPath);
            var htmlContent = Markdig.Markdown.ToHtml(markdownContent);
        
            var article = new
            {
                Title = "Mobile Forensics: An Introduction",
                Description = "Taking a look at how we can use mobile forensics to extract data from mobile devices.",
                Body = htmlContent,
                Author = "Crunchy",
                PublishedDate = dateString,
                ModifiedDate = dateString,
                Tags = new[] { "Mobile Forensics", "Data Extraction", "Digital Forensics" }
            };
            
            return Ok(article);
        }
        catch (FileNotFoundException fnf)
        {
            return NotFound("The requested Markdown file was not found.");
        }
        catch(IOException ioe)
        {
            return StatusCode(500, "An error occurred while reading the file.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
        }
    }
    
    
}