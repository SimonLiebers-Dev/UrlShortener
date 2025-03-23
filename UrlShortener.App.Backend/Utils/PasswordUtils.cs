using System.Security.Cryptography;
using System.Text;

namespace UrlShortener.App.Backend.Utils
{
    internal static class PasswordUtils
    {
        public static string GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string HashPassword(string password, string salt)
        {
            var saltedPassword = Encoding.UTF8.GetBytes(password + salt);
            var hash = SHA256.HashData(saltedPassword);
            return Convert.ToBase64String(hash);
        }
    }
}
