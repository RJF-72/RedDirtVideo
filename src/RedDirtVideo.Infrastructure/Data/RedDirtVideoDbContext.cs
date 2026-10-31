using Microsoft.EntityFrameworkCore;
using RedDirtVideo.Domain.Entities;

namespace RedDirtVideo.Infrastructure.Data;

public class RedDirtVideoDbContext : DbContext
{
    public RedDirtVideoDbContext(DbContextOptions<RedDirtVideoDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Song> Songs => Set<Song>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<AiJob> AiJobs => Set<AiJob>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.Email).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Role).HasMaxLength(50).HasDefaultValue("USER");
            entity.Property(x => x.FirstName).HasMaxLength(100);
            entity.Property(x => x.LastName).HasMaxLength(100);
            entity.Property(x => x.CompanyName).HasMaxLength(255);
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.ToTable("artists");
            entity.Property(x => x.StageName).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Genre).HasMaxLength(100);
            entity.HasIndex(x => x.UserId);
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.ToTable("songs");
            entity.Property(x => x.Title).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Genre).HasMaxLength(100);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("projects");
            entity.Property(x => x.Name).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Style).HasMaxLength(100);
            entity.Property(x => x.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.ToTable("venues");
            entity.Property(x => x.Name).HasMaxLength(255).IsRequired();
            entity.Property(x => x.VenueType).HasMaxLength(100);
        });

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.ToTable("assets");
            entity.Property(x => x.Filename).HasMaxLength(500).IsRequired();
            entity.Property(x => x.FileType).HasMaxLength(50);
            entity.Property(x => x.MimeType).HasMaxLength(255);
        });

        modelBuilder.Entity<AiJob>(entity =>
        {
            entity.ToTable("ai_jobs");
            entity.Property(x => x.JobType).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(50).IsRequired();
            entity.Property(x => x.QueueName).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("payments");
            entity.Property(x => x.Currency).HasMaxLength(10).IsRequired();
            entity.Property(x => x.Provider).HasMaxLength(50).IsRequired();
            entity.Property(x => x.ProviderTransactionId).HasMaxLength(255).IsRequired();
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("audit_logs");
            entity.Property(x => x.Action).HasMaxLength(255).IsRequired();
            entity.Property(x => x.EntityType).HasMaxLength(100);
        });

        modelBuilder.Entity<Venue>().HasData(
            new Venue
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Red Dirt Venue - Main Stage",
                VenueType = "Concert",
                Capacity = 5000,
                Description = "Flagship Texas-inspired venue for country and Red Dirt performances.",
                Active = true,
                CreatedAt = DateTime.UtcNow
            },
            new Venue
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Red Dirt Venue - Festival Grounds",
                VenueType = "Festival",
                Capacity = 25000,
                Description = "Outdoor festival grounds for large-country and Red Dirt experiences.",
                Active = true,
                CreatedAt = DateTime.UtcNow
            },
            new Venue
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Red Dirt Venue - Acoustic Room",
                VenueType = "Acoustic",
                Capacity = 500,
                Description = "Intimate performance room built for acoustic songwriter sessions.",
                Active = true,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
