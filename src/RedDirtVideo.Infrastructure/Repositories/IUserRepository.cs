using RedDirtVideo.Domain.Entities;

namespace RedDirtVideo.Infrastructure.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
}

public interface IArtistRepository : IRepository<Artist>
{
    Task<IEnumerable<Artist>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}

public interface ISongRepository : IRepository<Song>
{
    Task<IEnumerable<Song>> GetByArtistIdAsync(Guid artistId, CancellationToken cancellationToken = default);
}

public interface IProjectRepository : IRepository<Project>
{
    Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Project?> GetByIdWithDetailsAsync(Guid projectId, CancellationToken cancellationToken = default);
}

public interface IVenueRepository : IRepository<Venue>
{
    Task<IEnumerable<Venue>> GetActiveAsync(CancellationToken cancellationToken = default);
}

public interface IAiJobRepository : IRepository<AiJob>
{
    Task<IEnumerable<AiJob>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
