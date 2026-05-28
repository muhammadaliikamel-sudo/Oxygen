using System.ComponentModel.DataAnnotations.Schema;

namespace Oxygen.Models
{
    [Table("users")]
    public class User : BaseEntity
    {
        [Column("id")]
        public Guid Id { get; set; }

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

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;
    }
}