using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedDirtVideo.API.DTOs;
using RedDirtVideo.API.Services;
using RedDirtVideo.Domain.Entities;
using RedDirtVideo.Infrastructure.Data;

namespace RedDirtVideo.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RedDirtVideoDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthController(
        RedDirtVideoDbContext dbContext,
        IConfiguration configuration,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
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
            PasswordHash = _passwordHasher.Hash(request.Password),
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

        return Ok(new AuthResponse
        {
            UserId = user.Id,
            Token = _tokenService.Generate(user),
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

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash ?? string.Empty))
        {
            return Unauthorized();
        }

        return Ok(new AuthResponse
        {
            UserId = user.Id,
            Token = _tokenService.Generate(user),
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
}
