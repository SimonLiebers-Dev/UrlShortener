using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UrlShortener.Test.Frontend.Utils
{
    internal static class JwtUtils
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Vulnerability", "S6781:JWT secret keys should not be disclosed", Justification = "Testing")]
        public static string GenerateTestToken(string email)
        {
            var secretKey = "ThisIsAKeyForTestingAndHasAtLeast128Bits";
            var key = Encoding.UTF8.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new(ClaimTypes.Email, email),
                    new(ClaimTypes.Name, email)
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "Issuer",
                Audience = "Audience",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
