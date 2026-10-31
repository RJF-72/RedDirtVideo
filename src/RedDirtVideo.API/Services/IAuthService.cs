using RedDirtVideo.Domain.Entities;
using RedDirtVideo.Infrastructure.Repositories;

namespace RedDirtVideo.API.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(string email, string password, string? firstName, string? lastName, CancellationToken cancellationToken = default);
    Task<User?> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    string GenerateToken(User user);
}
