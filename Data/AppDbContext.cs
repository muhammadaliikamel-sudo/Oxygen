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
    }
}