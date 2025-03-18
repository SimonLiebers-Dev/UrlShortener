using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Backend.Utils;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IJwtTokenGenerator JwtTokenGenerator, AppDbContext DbContext) : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = DbContext.Users.SingleOrDefault(u => u.Email == request.Email);
            if (user == null)
                return Unauthorized("Invalid email or password");

            string hashedPassword = PasswordUtils.HashPassword(request.Password, user.Salt);
            if (hashedPassword != user.PasswordHash)
                return Unauthorized("Invalid email or password");

            var token = JwtTokenGenerator.GenerateToken(user.Email);
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("E-Mail and Passwort are required");

            if (await DbContext.Users.AnyAsync(u => u.Email == request.Email))
                return Conflict("A user with this E-Mail already exists");

            string salt = PasswordUtils.GenerateSalt();
            string hashedPassword = PasswordUtils.HashPassword(request.Password, salt);

            var newUser = new User
            {
                Email = request.Email,
                PasswordHash = hashedPassword,
                Salt = salt
            };

            DbContext.Users.Add(newUser);
            await DbContext.SaveChangesAsync();

            return Ok("User successfully registered");
        }
    }
}
