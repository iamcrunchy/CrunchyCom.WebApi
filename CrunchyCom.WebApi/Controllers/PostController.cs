using CrunchyCom.Business.Services;
using CrunchyCom.Data.Models;
using Microsoft.AspNetCore.Authorization;
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
    public IActionResult GetPostById(string id)
    {
        var post = _postService.GetPostById(id);
        return post != null ? Ok(post) : NotFound();
    }

    [Authorize]
    [HttpPost]
    public IActionResult CreatePost([FromBody] Post post)
    {
        _postService.CreatePost(post);
        return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
    }

    [Authorize]
    [HttpPut("{id}")]
    public IActionResult UpdatePost(string id, [FromBody] Post post)
    {
        if (id != post.Id) return BadRequest();
        _postService.UpdatePost(post);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult DeletePost(string id)
    {
        _postService.DeletePost(id);
        return NoContent();
    }
}
