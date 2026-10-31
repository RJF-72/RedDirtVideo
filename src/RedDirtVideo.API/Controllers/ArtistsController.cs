using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RedDirtVideo.API.DTOs;
using RedDirtVideo.Domain.Entities;
using RedDirtVideo.Infrastructure.Data;

namespace RedDirtVideo.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RedDirtVideoDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AuthController(RedDirtVideoDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email.Trim()))
        {
            return Conflict("An account with this email already exists.");
        }

        var user = new User
        {
            Email = request.Email.Trim(),
            PasswordHash = HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = "USER",
            EmailVerified = true,
            Active = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        return Ok(new AuthResponse
        {
            UserId = user.Id,
            Token = token,
            ExpiresIn = 86400
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email.Trim());
        if (user is null)
        {
            return Unauthorized();
        }

        if (!VerifyPassword(request.Password, user.PasswordHash ?? string.Empty))
        {
            return Unauthorized();
        }

        var token = GenerateJwtToken(user);

        return Ok(new AuthResponse
        {
            UserId = user.Id,
            Token = token,
            ExpiresIn = 86400
        });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<object>> Me()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
        var user = await _dbContext.Users.FindAsync(userId);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(new
        {
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Role,
            user.CompanyName
        });
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? "super-secret-key-change-this-in-production-1234567890";
        var issuer = _configuration["Jwt:Issuer"] ?? "RedDirtVideo";
        var audience = _configuration["Jwt:Audience"] ?? "RedDirtVideoClients";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private static bool VerifyPassword(string password, string storedHash)
    {
        return HashPassword(password) == storedHash;
    }
}
