using Microsoft.EntityFrameworkCore;
using RedDirtVideo.Domain.Entities;

namespace RedDirtVideo.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(RedDirtVideoDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(u => u.Email == email, cancellationToken);
    }
}

public class ArtistRepository : GenericRepository<Artist>, IArtistRepository
{
    public ArtistRepository(RedDirtVideoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Artist>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}

public class SongRepository : GenericRepository<Song>, ISongRepository
{
    public SongRepository(RedDirtVideoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Song>> GetByArtistIdAsync(Guid artistId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(s => s.ArtistId == artistId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}

public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    public ProjectRepository(RedDirtVideoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Project?> GetByIdWithDetailsAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
    }
}

public class VenueRepository : GenericRepository<Venue>, IVenueRepository
{
    public VenueRepository(RedDirtVideoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Venue>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(v => v.Active)
            .OrderBy(v => v.Name)
            .ToListAsync(cancellationToken);
    }
}

public class AiJobRepository : GenericRepository<AiJob>, IAiJobRepository
{
    public AiJobRepository(RedDirtVideoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<AiJob>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(j => j.UserId == userId)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
