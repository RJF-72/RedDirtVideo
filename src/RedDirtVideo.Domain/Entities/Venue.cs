namespace RedDirtVideo.Domain.Entities;

public class Venue
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? VenueType { get; set; }
    public int? Capacity { get; set; }
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
