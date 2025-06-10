using Microsoft.EntityFrameworkCore;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Utils
{
    internal static class DataSeeder
    {
        private static User GetDemoUser(string email, string password)
        {
            var salt = PasswordUtils.GenerateSalt();
            return new User
            {
                Email = email,
                PasswordHash = PasswordUtils.HashPassword(password, salt),
                Salt = salt,
            };
        }

        private static List<UrlMapping> GetDemoUrlMappings(User user)
        {
            var mapping1 = new UrlMapping()
            {
                CreatedAt = new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                LongUrl = "https://sonarcloud.io/project/overview?id=SimonLiebers-Dev_UrlShortener",
                Name = "SonarCloud",
                Path = "12345",
                User = user.Email,
                RedirectLogs = []
            };

            var mapping2 = new UrlMapping()
            {
                CreatedAt = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                LongUrl = "https://github.com/SimonLiebers-Dev/UrlShortener",
                Name = "GitHub",
                Path = "23456",
                User = user.Email,
                RedirectLogs = []
            };

            var mapping3 = new UrlMapping()
            {
                CreatedAt = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                LongUrl = "https://urlshortener.readthedocs.io/en/latest/",
                Name = "ReadTheDocs",
                Path = "34567",
                User = user.Email,
                RedirectLogs = []
            };

            return [mapping1, mapping2, mapping3];
        }

        private static List<RedirectLog> GetDemoRedirectLogs(UrlMapping urlMapping, DateTime fromDate, DateTime toDate, int numberOfEntries = 5)
        {
            var random = new Random();
            var logs = new List<RedirectLog>();

            // Sample data pools (same as before)
            string[] ipAddresses = { "Dummy IP 1", "Dummy IP 2", "Dummy IP 3", "Dummy IP 4" };
            string[] userAgents = { "Mozilla/5.0", "Chrome/113.0", "Safari/537.36", "Edge/91.0" };
            string[] browserFamilies = { "Chrome", "Firefox", "Edge", "Safari" };
            string[] clientEngines = { "Blink", "Gecko", "WebKit" };
            string[] clientNames = { "Chrome", "Firefox", "Safari", "Edge" };
            string[] clientTypes = { "browser", "app" };
            string[] deviceBrands = { "Apple", "Samsung", "Google", "Huawei" };
            string[] deviceModels = { "iPhone 14", "Galaxy S23", "Pixel 7", "P40" };
            string[] deviceTypes = { "smartphone", "desktop", "tablet" };
            string[] osNames = { "iOS", "Windows", "Android", "macOS" };
            string[] osVersions = { "16.0", "10", "13", "12.5" };
            string[] osFamilies = { "iOS", "Windows", "Linux", "macOS" };

            for (int i = 0; i < numberOfEntries; i++)
            {
                // Randomly pick a timestamp
                var accessedAt = fromDate.AddSeconds(random.Next((int)(toDate - fromDate).TotalSeconds));

                // Randomly decide how many clicks to simulate for this timestamp (e.g., 1–5)
                int clicks = random.Next(1, 10);

                for (int j = 0; j < clicks; j++)
                {
                    logs.Add(new RedirectLog
                    {
                        UrlMappingId = urlMapping.Id,
                        UrlMapping = urlMapping,
                        IpAddress = ipAddresses[random.Next(ipAddresses.Length)],
                        UserAgent = userAgents[random.Next(userAgents.Length)],
                        AccessedAt = accessedAt, // shared timestamp for all clicks in this iteration
                        BrowserFamily = browserFamilies[random.Next(browserFamilies.Length)],
                        ClientEngine = clientEngines[random.Next(clientEngines.Length)],
                        ClientName = clientNames[random.Next(clientNames.Length)],
                        ClientType = clientTypes[random.Next(clientTypes.Length)],
                        DeviceBrand = deviceBrands[random.Next(deviceBrands.Length)],
                        DeviceModel = deviceModels[random.Next(deviceModels.Length)],
                        DeviceType = deviceTypes[random.Next(deviceTypes.Length)],
                        OsName = osNames[random.Next(osNames.Length)],
                        OsVersion = osVersions[random.Next(osVersions.Length)],
                        OsFamily = osFamilies[random.Next(osFamilies.Length)]
                    });
                }
            }

            return logs;
        }

        public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken)
        {
            var demoUser = GetDemoUser("demo@test.com", "Test123");
            var targetUser = await context.Set<User>().FirstOrDefaultAsync(b => b.Email == demoUser.Email, cancellationToken);

            // Only seed if user not exists
            if (targetUser != null)
            {
                return;
            }

            // Seed user
            context.Set<User>().Add(demoUser);
            await context.SaveChangesAsync(cancellationToken);

            // Seed urls
            var demoMappings = GetDemoUrlMappings(demoUser);
            context.Set<UrlMapping>().AddRange(demoMappings);
            await context.SaveChangesAsync(cancellationToken);

            // Seed redirect logs
            var demoLogs = new List<RedirectLog>();
            foreach (var mapping in demoMappings)
            {
                var mappingLogs = GetDemoRedirectLogs(mapping, mapping.CreatedAt, new DateTime(2025, 6, 20, 0, 0, 0, DateTimeKind.Utc));
                demoLogs.AddRange(mappingLogs);
            }
            context.Set<RedirectLog>().AddRange(demoLogs);
            await context.SaveChangesAsync(cancellationToken);
        }

        public static void Seed(DbContext context)
        {
            var demoUser = GetDemoUser("demo@test.com", "Test123");
            var targetUser = context.Set<User>().FirstOrDefault(b => b.Email == demoUser.Email);

            // Only seed if user not exists
            if (targetUser != null)
            {
                return;
            }

            // Seed user
            context.Set<User>().Add(demoUser);
            context.SaveChanges();

            // Seed urls
            var demoMappings = GetDemoUrlMappings(demoUser);
            context.Set<UrlMapping>().AddRange(demoMappings);
            context.SaveChanges();

            // Seed redirect logs
            var demoLogs = new List<RedirectLog>();
            foreach (var mapping in demoMappings)
            {
                var mappingLogs = GetDemoRedirectLogs(mapping, mapping.CreatedAt, new DateTime(2025, 6, 20, 0, 0, 0, DateTimeKind.Utc));
                demoLogs.AddRange(mappingLogs);
            }
            context.Set<RedirectLog>().AddRange(demoLogs);
            context.SaveChanges();
        }
    }
}
