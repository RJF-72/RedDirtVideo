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
    public DbSet<Lyric> Lyrics => Set<Lyric>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Storyboard> Storyboards => Set<Storyboard>();
    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<AiJob> AiJobs => Set<AiJob>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
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
            entity.HasIndex(x => x.ArtistId);
        });

        modelBuilder.Entity<Lyric>(entity =>
        {
            entity.ToTable("lyrics");
            entity.HasIndex(x => x.SongId);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("projects");
            entity.Property(x => x.Name).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Style).HasMaxLength(100);
            entity.Property(x => x.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Storyboard>(entity =>
        {
            entity.ToTable("storyboards");
            entity.HasIndex(x => x.ProjectId);
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
            entity.Property(x => x.QueueName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.ToTable("subscription_plans");
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.MaxResolution).HasMaxLength(20);
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.ToTable("subscriptions");
            entity.Property(x => x.Status).HasMaxLength(50).HasDefaultValue("ACTIVE");
            entity.HasIndex(x => x.UserId);
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
                Description = "Flagship Texas-inspired main stage with lighting and crowd energy.",
                Active = true,
                CreatedAt = DateTime.UtcNow
            },
            new Venue
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Red Dirt Venue - Festival Grounds",
                VenueType = "Festival",
                Capacity = 25000,
                Description = "Wide festival grounds built for large-scale country performances.",
                Active = true,
                CreatedAt = DateTime.UtcNow
            },
            new Venue
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Red Dirt Venue - Acoustic Room",
                VenueType = "Acoustic",
                Capacity = 500,
                Description = "Intimate venue for acoustic songwriter moments.",
                Active = true,
                CreatedAt = DateTime.UtcNow
            },
            new Venue
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "Texas Dance Hall",
                VenueType = "DanceHall",
                Capacity = 800,
                Description = "Classic country dance hall atmosphere.",
                Active = true,
                CreatedAt = DateTime.UtcNow
            },
            new Venue
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Name = "Texas Rodeo Arena",
                VenueType = "Arena",
                Capacity = 12000,
                Description = "Large arena with full production and crowd spectacle.",
                Active = true,
                CreatedAt = DateTime.UtcNow
            },
            new Venue
            {
                Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                Name = "Outdoor Worship Venue",
                VenueType = "Worship",
                Capacity = 3000,
                Description = "Warm worship venue for Christian Country and church performances.",
                Active = true,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
