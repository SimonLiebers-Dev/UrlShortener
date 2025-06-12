using UrlShortener.App.Backend.Utils;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.Test.End2End.Data
{
    public static class TestData
    {
        public static List<User> GetDefaultTestUsers()
        {
            var passwordSalt = PasswordUtils.GenerateSalt();
            var testUser1 = new User
            {
                Email = "test@gmail.com",
                PasswordHash = PasswordUtils.HashPassword("TestPassword", passwordSalt),
                Salt = passwordSalt
            };

            var testUser2 = new User
            {
                Email = "test2@gmail.com",
                PasswordHash = PasswordUtils.HashPassword("TestPassword2", passwordSalt),
                Salt = passwordSalt
            };

            return [testUser1, testUser2];
        }

        public static List<UrlMapping> GetDefaultTestUrlMappings()
        {
            var testUrlMapping = new UrlMapping
            {
                CreatedAt = DateTime.UtcNow,
                LongUrl = "https://google.com",
                Path = "test",
                Name = "Test"
            };

            return [testUrlMapping];
        }

        public static List<UrlMapping> GetDefaultTestUrlMappingsForUsers(List<User> users)
        {
            var mappings = new List<UrlMapping>();
            foreach (var user in users)
            {
                var testUrlMapping = new UrlMapping
                {
                    User = user.Email,
                    CreatedAt = DateTime.UtcNow,
                    LongUrl = $"https://example-{user.Id}.com",
                    Path = $"test_user_{user.Id}",
                    Name = $"TestMapping by {user.Email}"
                };
                mappings.Add(testUrlMapping);
            }

            return mappings;
        }

        public static List<UrlMapping> GetDefaultTestUrlMappingsForUsers()
        {
            var users = GetDefaultTestUsers();
            return GetDefaultTestUrlMappingsForUsers(users);
        }
    }
}
