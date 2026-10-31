namespace RedDirtVideo.Domain.Entities;

public class Storyboard
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; }
    public string? Status { get; set; }
    public string? GeneratedPrompt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
