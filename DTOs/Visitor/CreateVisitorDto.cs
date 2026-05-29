namespace Oxygen.DTOs.Visitor
{
    public class CreateVisitorDto
    {
        public required string FullName { get; set; }

        public required string Email { get; set; }

        public required string Phone { get; set; }

        public required string Category { get; set; }

        public required string Source { get; set; }

        public required string Language { get; set; }
    }
}