namespace Oxygen.Models
{
    public class Document
    {
        public Guid Id { get; set; }
        public required string FileName { get; set; }
        public required string BucketName { get; set; }
        public required string FileUrl { get; set; }
        public required string MimeType { get; set; }
        public long Size { get; set; }
        public Guid UploadedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
