using CrunchyCom.Business.Services;
using CrunchyCom.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrunchyCom.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public IActionResult GetAllPosts() => Ok(_postService.GetAllPosts());

    [HttpGet("{id}")]
    public IActionResult GetPostById(int id)
    {
        var post = _postService.GetPostById(id);
        return post != null ? Ok(post) : NotFound();
    }

    [HttpPost]
    public IActionResult CreatePost([FromBody] Post post)
    {
        _postService.CreatePost(post);
        return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
    }

    [HttpPut("{id}")]
    public IActionResult UpdatePost(int id, [FromBody] Post post)
    {
        if (id != post.Id) return BadRequest();
        _postService.UpdatePost(post);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletePost(int id)
    {
        _postService.DeletePost(id);
        return NoContent();
    }
}
