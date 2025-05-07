using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
    private readonly UserRepository _userRepository;
    private readonly ILogger<AuthController> _logger;
    
    public AuthController(IConfiguration configuration, 
        UserRepository userRepository, 
        ILogger<AuthController> logger)
    {
        _logger = logger;
        _configuration = configuration;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _userRepository.GetByUserName("admin");

        if (user == null)
        {
            return Unauthorized();
        }
        
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized();
        }

        var token = GenerateJwtToken(user.UserName);
        return Ok(new { Token = token });
    }

    /// <summary>
    /// Generates a JSON Web Token (JWT) for a given username.
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
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: new[] { new Claim(ClaimTypes.Name, username) },
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}