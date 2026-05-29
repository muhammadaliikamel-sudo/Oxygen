using System.ComponentModel.DataAnnotations.Schema;

namespace Oxygen.Models
{
    [Table("visitors")]
    public class Visitor
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("category")]
        public string? Category { get; set; }

        [Column("source")]
        public string? Source { get; set; }

        [Column("language")]
        public string? Language { get; set; }

        [Column("created_by")]
        public Guid? CreatedBy { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

        [Column("password_hash")]
        public string? PasswordHash { get; set; }
    }
}