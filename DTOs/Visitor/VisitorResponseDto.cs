namespace Oxygen.DTOs.Visitor
{
    public class VisitorResponseDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Category { get; set; }

        public string Source { get; set; }

        public string Language { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}