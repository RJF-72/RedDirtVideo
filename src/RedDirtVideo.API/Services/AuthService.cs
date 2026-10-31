using System.Security.Claims;
using RedDirtVideo.Domain.Entities;
using RedDirtVideo.Infrastructure.Repositories;

namespace RedDirtVideo.API.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<User?> RegisterAsync(string email, string password, string? firstName, string? lastName, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();
        if (await _unitOfWork.Users.ExistsByEmailAsync(normalizedEmail, cancellationToken))
        {
            return null;
        }

        var user = new User
        {
            Email = normalizedEmail,
            PasswordHash = _passwordHasher.Hash(password),
            FirstName = firstName,
            LastName = lastName,
            Role = "USER",
            EmailVerified = true,
            Active = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<User?> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email.Trim(), cancellationToken);
        if (user is null)
        {
            return null;
        }

        if (!_passwordHasher.Verify(password, user.PasswordHash ?? string.Empty))
        {
            return null;
        }

        return user;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
    }

    public string GenerateToken(User user)
    {
        return _tokenService.Generate(user);
    }
}
