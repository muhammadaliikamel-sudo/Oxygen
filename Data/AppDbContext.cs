using Microsoft.EntityFrameworkCore;
using Oxygen.Models;

namespace Oxygen.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Visitor> Visitors { get; set; }

        public DbSet<Interaction> Interactions { get; set; }

        public DbSet<CTA> CTAs { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("users");

            modelBuilder.Entity<Visitor>()
                .ToTable("visitors");

            modelBuilder.Entity<Interaction>()
                .ToTable("interactions");

            modelBuilder.Entity<CTA>()
                .ToTable("ctas");

            // User
            modelBuilder.Entity<User>()
                .Property(u => u.FullName)
                .HasColumnName("full_name");

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .HasColumnName("password_hash");

            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .HasColumnName("is_active");

            modelBuilder.Entity<User>()
                .Property(u => u.LastLogin)
                .HasColumnName("last_login");

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasColumnName("created_at");

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasColumnName("updated_at");

            modelBuilder.Entity<User>()
                .Property(u => u.IsDeleted)
                .HasColumnName("is_deleted");

            // Visitor
            modelBuilder.Entity<Visitor>()
                .Property(v => v.FullName)
                .HasColumnName("full_name");

            modelBuilder.Entity<Visitor>()
                .Property(v => v.CreatedBy)
                .HasColumnName("created_by");

            modelBuilder.Entity<Visitor>()
                .Property(v => v.PasswordHash)
                .HasColumnName("password_hash");

            modelBuilder.Entity<Visitor>()
                .Property(v => v.CreatedAt)
                .HasColumnName("created_at");

            modelBuilder.Entity<Visitor>()
                .Property(v => v.UpdatedAt)
                .HasColumnName("updated_at");

            modelBuilder.Entity<Visitor>()
                .Property(v => v.IsDeleted)
                .HasColumnName("is_deleted");
        }
    }
}