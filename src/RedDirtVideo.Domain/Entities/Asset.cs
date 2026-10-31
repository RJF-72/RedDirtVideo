namespace RedDirtVideo.Domain.Entities;

public class Asset
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Filename { get; set; } = string.Empty;
    public string? FileType { get; set; }
    public string? MimeType { get; set; }
    public long? FileSize { get; set; }
    public string? StorageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
