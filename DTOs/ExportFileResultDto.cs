namespace Oxygen.DTOs
{
    public class ExportFileResultDto
    {
        public required byte[] Content { get; set; }
        public required string ContentType { get; set; }
        public required string FileName { get; set; }
        public required string Format { get; set; }
    }
}
