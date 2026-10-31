using RedDirtVideo.Domain.Entities;
using RedDirtVideo.Infrastructure.Data;

namespace RedDirtVideo.Infrastructure.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IArtistRepository Artists { get; }
    ISongRepository Songs { get; }
    IProjectRepository Projects { get; }
    IVenueRepository Venues { get; }
    IAiJobRepository AiJobs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly RedDirtVideoDbContext _context;

    public UnitOfWork(RedDirtVideoDbContext context)
    {
        _context = context;
        Users = new UserRepository(context);
        Artists = new ArtistRepository(context);
        Songs = new SongRepository(context);
        Projects = new ProjectRepository(context);
        Venues = new VenueRepository(context);
        AiJobs = new AiJobRepository(context);
    }

    public IUserRepository Users { get; }
    public IArtistRepository Artists { get; }
    public ISongRepository Songs { get; }
    public IProjectRepository Projects { get; }
    public IVenueRepository Venues { get; }
    public IAiJobRepository AiJobs { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
