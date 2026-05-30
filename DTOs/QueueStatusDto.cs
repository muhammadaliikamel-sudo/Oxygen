namespace Oxygen.DTOs
{
    public class QueueStatusDto
    {
        public required string QueueName { get; set; }
        public int PendingCount { get; set; }
    }
}
