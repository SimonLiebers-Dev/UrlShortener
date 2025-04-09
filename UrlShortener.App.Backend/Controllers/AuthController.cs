using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Backend.Utils;
using UrlShortener.App.Shared.Dto;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IJwtTokenGenerator JwtTokenGenerator, AppDbContext DbContext) : ControllerBase
    {
        /// <summary>
        /// Login endpoint to authenticate a user and return a JWT token.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <returns>A JWT token if authentication is successful, otherwise an Unauthorized error.</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var user = DbContext.Users.SingleOrDefault(u => u.Email == request.Email);
            if (user == null)
                return Unauthorized("Invalid email or password");

            string hashedPassword = PasswordUtils.HashPassword(request.Password, user.Salt);
            if (hashedPassword != user.PasswordHash)
                return Unauthorized("Invalid email or password");

            var token = JwtTokenGenerator.GenerateToken(user.Email);
            return Ok(new LoginResponseDto()
            {
                Token = token
            });
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="request">The registration request containing email and password.</param>
        /// <returns>Response indicating success or failure of registration.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new RegisterResponseDto() { Success = false, ErrorType = RegisterErrorType.MissingEmailOrPassword });

            var email = new EmailAddressAttribute();
            if (!email.IsValid(request.Email))
                return BadRequest(new RegisterResponseDto() { Success = false, ErrorType = RegisterErrorType.MissingEmailOrPassword });

            if (await DbContext.Users.AnyAsync(u => u.Email == request.Email))
                return Conflict(new RegisterResponseDto() { Success = false, ErrorType = RegisterErrorType.EmailAlreadyExists });

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

            return Ok(new RegisterResponseDto());
        }
    }
}
