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
        }
    }
}