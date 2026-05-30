namespace Oxygen.DTOs
{
    public class DashboardMetricDto
    {
        public required string MetricName { get; set; }
        public required string MetricValue { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
