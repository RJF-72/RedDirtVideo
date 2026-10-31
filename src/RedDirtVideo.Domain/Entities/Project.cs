namespace RedDirtVideo.Domain.Entities;

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid ArtistId { get; set; }
    public Guid? SongId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Style { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
