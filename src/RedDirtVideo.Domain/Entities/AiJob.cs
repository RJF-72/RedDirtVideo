namespace RedDirtVideo.Domain.Entities;

public class AiJob
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid? ProjectId { get; set; }
    public string JobType { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
    public int Priority { get; set; } = 5;
    public string Status { get; set; } = "QUEUED";
    public int Progress { get; set; } = 0;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
