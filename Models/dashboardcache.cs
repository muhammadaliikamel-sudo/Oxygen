namespace Oxygen.Models
{
    public class DashboardCache
    {
        public Guid Id { get; set; }
        public required string MetricName { get; set; }
        public required string MetricValue { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
