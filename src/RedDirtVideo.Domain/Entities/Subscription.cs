namespace RedDirtVideo.Domain.Entities;

public class SubscriptionPlan
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public decimal MonthlyPrice { get; set; }
    public int MonthlyCredits { get; set; }
    public int? MaxProjects { get; set; }
    public int? MaxStorageGb { get; set; }
    public string? MaxResolution { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
