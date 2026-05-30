namespace Oxygen.DTOs
{
    public class DocumentUploadResponseDto
    {
        public Guid DocumentId { get; set; }
        public required string FileUrl { get; set; }
        public required string Bucket { get; set; }
        public required string FileName { get; set; }
    }
}
