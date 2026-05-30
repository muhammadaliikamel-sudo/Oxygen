namespace Oxygen.Models
{
    public class ExportLog
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public required string FileName { get; set; }
        public required string Format { get; set; }
        public required string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
