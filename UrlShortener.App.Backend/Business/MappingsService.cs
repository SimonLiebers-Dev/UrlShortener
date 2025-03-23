using Microsoft.EntityFrameworkCore;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Business
{
    internal class MappingsService(AppDbContext DbContext) : IMappingsService
    {
        private readonly Random Random = new();
        public async Task<UrlMapping?> GetMappingByPath(string path)
        {
            return await DbContext.UrlMappings.FirstOrDefaultAsync(u => u.Path.Equals(path));
        }

        public async Task<UrlMapping?> CreateMapping(string longUrl, string name, string email)
        {
            var path = await GetUniqueRandomStringAsync();

            var urlMapping = new UrlMapping
            {
                LongUrl = longUrl,
                Path = path,
                CreatedAt = DateTime.UtcNow,
                User = email,
                Name = name
            };

            await DbContext.UrlMappings.AddAsync(urlMapping);
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

        public async Task<bool> DeleteMapping(string email, int mappingId)
        {
            var urlMapping = await DbContext.UrlMappings
                .FirstOrDefaultAsync(m => m.User != null && m.User.Equals(email) && m.Id == mappingId);

            if (urlMapping == null)
                return false;

            DbContext.UrlMappings.Remove(urlMapping);
            await DbContext.SaveChangesAsync();

            return true;
        }
    }
}
