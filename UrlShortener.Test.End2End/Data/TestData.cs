using UrlShortener.App.Backend.Utils;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.Test.End2End.Data
{
    public static class TestData
    {
        public static List<User> GetDefaultTestUsers()
        {
            var passwordSalt = PasswordUtils.GenerateSalt();
            var testUser = new User()
            {
                Email = "test@gmail.com",
                PasswordHash = PasswordUtils.HashPassword("TestPassword", passwordSalt),
                Salt = passwordSalt
            };

            return [testUser];
        }
    }
}
