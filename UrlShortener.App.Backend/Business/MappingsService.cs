using Microsoft.EntityFrameworkCore;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Business
{
    public class MappingsService(AppDbContext DbContext) : IMappingsService
    {
        private readonly Random Random = new();
        public async Task<UrlMapping?> GetMappingByPath(string path)
        {
            var mappings = await DbContext.UrlMappings.ToListAsync();
            var found = await DbContext.UrlMappings.FirstOrDefaultAsync(u => u.Path.Equals(path));
            return found;
        }

        public async Task<UrlMapping?> CreateMapping(string longUrl, string? email = null)
        {
            var path = await GetUniqueRandomStringAsync();

            var urlMapping = new UrlMapping
            {
                LongUrl = longUrl,
                Path = path,
                CreatedAt = DateTime.UtcNow,
                User = email
            };

            DbContext.UrlMappings.Add(urlMapping);
            await DbContext.SaveChangesAsync();

            return urlMapping;
        }

        private async Task<string> GetUniqueRandomStringAsync()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            string result;
            do
            {
                // Create random string
                result = new string([.. Enumerable.Repeat(chars, 6).Select(s => s[Random.Next(s.Length)])]);

                // Check if the generated string already exists
            } while (await GetMappingByPath(result) != null);

            return result;
        }

        public async Task<List<UrlMapping>?> GetMappingsByUser(string email)
        {
            return await DbContext.UrlMappings
                .Where(m => m.User != null && m.User.Equals(email))
                .Include(m => m.RedirectLogs) 
                .ToListAsync();
        }
    }
}
