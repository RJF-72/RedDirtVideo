namespace RedDirtVideo.Domain.Entities;

public class Song
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ArtistId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Genre { get; set; }
    public int? Bpm { get; set; }
    public int? DurationSeconds { get; set; }
    public Guid? AudioAssetId { get; set; }
    public Guid? CoverArtAssetId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
