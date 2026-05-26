namespace Oxygen.Models
{
    public class User : BaseEntity
    {
        public required string FullName { get; set; }

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public bool is_active { get; set; }

        public DateTime? last_login { get; set; }
    }
}       
