using Microsoft.EntityFrameworkCore;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(); // E-Mail must be unique
        }
    }
}
