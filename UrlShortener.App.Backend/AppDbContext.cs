using Microsoft.EntityFrameworkCore;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend
{
    /// <summary>
    /// Database context for the application.
    /// </summary>
    /// <param name="options">The options for the database context.</param>
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        /// <summary>
        /// Gets or sets the database table of users. 
        /// Each <see cref="User"/> represents an individual user of the URL shortener system.
        /// </summary>
        public virtual DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the database table of URL mappings. 
        /// Each <see cref="UrlMapping"/> represents a shortened URL and its corresponding original URL with additional properties.
        /// </summary>
        public virtual DbSet<UrlMapping> UrlMappings { get; set; }

        /// <summary>
        /// Gets or sets the database table of redirect logs. 
        /// Each <see cref="RedirectLog"/> records information about a redirection event, including timestamp and IP address.
        /// </summary>
        public virtual DbSet<RedirectLog> RedirectLogs { get; set; }

        /// <summary>
        /// Configures the entity relationships and constraints using the model builder.
        /// This method is called when the model for a derived context has been initialized,
        /// but before the model has been locked down and used to initialize the context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for the context.</param>
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
