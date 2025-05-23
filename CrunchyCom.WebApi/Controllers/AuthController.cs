using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CrunchyCom.Data.Models;
using CrunchyCom.Data.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CrunchyCom.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private readonly UserRepository _userRepository;

    public AuthController(IConfiguration configuration,
        UserRepository userRepository,
        ILogger<AuthController> logger)
    {
        _logger = logger;
        _configuration = configuration;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userRepository.GetByUserNameAsync(request.Email);

        if (user == null) return Unauthorized();

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return Unauthorized();

        var token = GenerateJwtToken(user.UserName);
        return Ok(new { Token = token, UserName = user.UserName });
    }

    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequest request)
    {
        // Check if admin already exists
        var existingUser = await _userRepository.GetByUserNameAsync(request.Username);
        if (existingUser != null)
            return BadRequest("Username already exists");

        // Hash password with BCrypt
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            UserName = request.Username,
            PasswordHash = passwordHash,
            Roles = ["Admin"],
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);
        return Ok("Admin user created successfully");
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    /// <summary>
    ///     Generates a JSON Web Token (JWT) for a given username.
    /// </summary>
    /// <param name="username">The username for which the JWT is generated.</param>
    /// <returns>A string representation of the generated JWT.</returns>
    private string GenerateJwtToken(string username)
    {
        var secret = _configuration["JwtSettings:Secret"];
        if (secret == null)
        {
            _logger.LogError("JWT secret is not configured.");
            throw new InvalidOperationException("JWT secret is not configured.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["JwtSettings:Issuer"],
            _configuration["JwtSettings:Audience"],
            new[] { new Claim(ClaimTypes.Name, username) },
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class RegisterRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}