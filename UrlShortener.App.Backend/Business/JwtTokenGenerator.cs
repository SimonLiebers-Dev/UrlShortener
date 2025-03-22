using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UrlShortener.App.Backend.Business
{
    public class JwtTokenGenerator(IConfiguration Config) : IJwtTokenGenerator
    {
        public string GenerateToken(string email)
        {
            var secretKey = Config["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("The JWT secret key is missing in the configuration.");
            }

            var key = Encoding.UTF8.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new(ClaimTypes.Email, email),
                    new(ClaimTypes.Name, email)
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = Config["JwtSettings:Issuer"],
                Audience = Config["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
