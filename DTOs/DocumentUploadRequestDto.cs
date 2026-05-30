using Microsoft.AspNetCore.Http;

namespace Oxygen.DTOs
{
    public class DocumentUploadRequestDto
    {
        public required IFormFile File { get; set; }
        public required string Bucket { get; set; }
        public string? EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public Guid? UploadedBy { get; set; }
    }
}
