using CrunchyCom.Business.Services;
using CrunchyCom.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrunchyCom.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetAllUsers() => Ok(_userService.GetAllUsers());

    [HttpGet("{id}")]
    public IActionResult GetUserById(string id)
    {
        var user = _userService.GetUserById(id);
        return user != null ? Ok(user) : NotFound();
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        _userService.CreateUser(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(string id, [FromBody] User user)
    {
        if (id != user.Id) return BadRequest();
        _userService.UpdateUser(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(string id)
    {
        _userService.DeleteUser(id);
        return NoContent();
    }
}