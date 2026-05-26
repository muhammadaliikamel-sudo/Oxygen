namespace Oxygen.Models
{
    public class Visitor : BaseEntity
    {
        public required string Name { get; set; }

        public required string email { get; set; }
        public required string Phone { get; set; }

        public required string Company { get; set; }

        public required string category { get; set; }

        public required string Source { get; set; }
    }
}