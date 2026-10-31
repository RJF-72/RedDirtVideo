using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedDirtVideo.API.DTOs;
using RedDirtVideo.API.Services;

namespace RedDirtVideo.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        var user = await _authService.RegisterAsync(request.Email, request.Password, request.FirstName, request.LastName);
        if (user is null)
        {
            return Conflict("An account with this email already exists.");
        }

        return Ok(new AuthResponse
        {
            UserId = user.Id,
            Token = _authService.GenerateToken(user),
            ExpiresIn = 86400
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _authService.LoginAsync(request.Email, request.Password);
        if (user is null)
        {
            return Unauthorized();
        }

        return Ok(new AuthResponse
        {
            UserId = user.Id,
            Token = _authService.GenerateToken(user),
            ExpiresIn = 86400
        });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<object>> Me()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
        var user = await _authService.GetUserByIdAsync(userId);

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
