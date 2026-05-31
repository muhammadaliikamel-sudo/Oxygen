using Microsoft.EntityFrameworkCore;
using Oxygen.Models;

namespace Oxygen.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Interaction> Interactions { get; set; }
        public DbSet<CTA> CTAs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Visitor>().ToTable("visitors");
            modelBuilder.Entity<Interaction>().ToTable("interactions");
            modelBuilder.Entity<CTA>().ToTable("ctas");

            //USER 
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Id).HasColumnName("id");
                entity.Property(u => u.FullName).HasColumnName("full_name");
                entity.Property(u => u.PasswordHash).HasColumnName("password_hash");
                entity.Property(u => u.IsActive).HasColumnName("is_active");
                entity.Property(u => u.LastLogin).HasColumnName("last_login");
                entity.Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
                entity.Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
                entity.Property(u => u.IsDeleted).HasColumnName("is_deleted");
            });

            //VISITOR 
            modelBuilder.Entity<Visitor>(entity =>
            {
                entity.Property(v => v.FullName).HasColumnName("full_name");
                entity.Property(v => v.CreatedBy).HasColumnName("created_by");
                entity.Property(v => v.PasswordHash).HasColumnName("password_hash");
                entity.Property(v => v.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
                entity.Property(v => v.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
                entity.Property(v => v.IsDeleted).HasColumnName("is_deleted");
            });

            //INTERACTION 
            modelBuilder.Entity<Interaction>(entity =>
            {
                entity.Property(i => i.Id).HasColumnName("id");
                entity.Property(i => i.VisitorId).HasColumnName("visitor_id");
                entity.Property(i => i.Notes).HasColumnName("notes");
                entity.Property(i => i.Type).HasColumnName("type");
                entity.Property(i => i.DurationMinutes).HasColumnName("duration_minutes");
                entity.Property(i => i.CreatedBy).HasColumnName("created_by");
                entity.Property(i => i.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
                entity.Property(i => i.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
                entity.Property(i => i.IsDeleted).HasColumnName("is_deleted");
            });

            //CTA
            modelBuilder.Entity<CTA>(entity =>
            {
                entity.Property(c => c.Id).HasColumnName("id");
                entity.Property(c => c.Status).HasColumnName("status");
                entity.Property(c => c.Type).HasColumnName("type");
                entity.Property(c => c.InteractionId).HasColumnName("interaction_id");
                entity.Property(c => c.OwnerId).HasColumnName("owner_id");
                entity.Property(c => c.DueDate).HasColumnName("due_date").HasColumnType("timestamp with time zone");
                entity.Property(c => c.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone");
                entity.Property(c => c.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone");
                entity.Property(c => c.SLAState).HasColumnName("sla_state");
                entity.Property(c => c.IsDeleted).HasColumnName("is_deleted");
            });
        }

        //  AUTO AUDIT
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var baseEntries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in baseEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            // Normalize ALL DateTime properties to UTC for Npgsql compatibility.
            // PostgreSQL "timestamp with time zone" requires Kind=Utc; Npgsql rejects Unspecified.
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.CurrentValue is DateTime dt && dt.Kind == DateTimeKind.Unspecified)
                        {
                            prop.CurrentValue = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                        }
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}