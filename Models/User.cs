using System.ComponentModel.DataAnnotations.Schema;

namespace Oxygen.Models
{
    [Table("users")]
    public class User : BaseEntity
    {
        [Column("full_name")]
        public required string FullName { get; set; }

        [Column("email")]
        public required string Email { get; set; }

        [Column("password_hash")]
        public required string PasswordHash { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("last_login")]
        public DateTime? LastLogin { get; set; }
    }
}