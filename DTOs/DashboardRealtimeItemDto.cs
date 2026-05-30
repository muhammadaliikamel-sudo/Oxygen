namespace Oxygen.DTOs;

public class DashboardRealtimeItemDto
{
    public required string Type { get; set; }
    public Guid EntityId { get; set; }
    public required string Title { get; set; }
    public DateTime CreatedAt { get; set; }
}
