using Microsoft.EntityFrameworkCore;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UrlMapping> UrlMappings { get; set; }
        public virtual DbSet<RedirectLog> RedirectLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<RedirectLog>()
                .HasOne(log => log.UrlMapping)
                .WithMany(mapping => mapping.RedirectLogs)
                .HasForeignKey(log => log.UrlMappingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
