using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Backend.Utils;
using UrlShortener.App.Shared.Dto;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Controllers
{
    /// <summary>
    /// Controller responsible for user authentication, including login and registration endpoints.
    /// </summary>
    /// <remarks>
    /// Relies on <see cref="IJwtTokenGenerator"/> for JWT creation and <see cref="AppDbContext"/> for user persistence.
    /// </remarks>
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IJwtTokenGenerator JwtTokenGenerator, AppDbContext DbContext) : ControllerBase
    {
        /// <summary>
        /// Authenticates a user and returns a JWT token if credentials are valid.
        /// </summary>
        /// <param name="request">The login request containing the user's email and password.</param>
        /// <returns>
        /// <see cref="OkObjectResult"/> with a <see cref="LoginResponseDto"/> containing the JWT token,
        /// or <see cref="UnauthorizedObjectResult"/> if the credentials are invalid.
        /// </returns>
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
        /// Registers a new user and stores their credentials securely.
        /// </summary>
        /// <param name="request">The registration request containing the user's email and password.</param>
        /// <returns>
        /// <see cref="OkObjectResult"/> with a successful <see cref="RegisterResponseDto"/> if registration succeeds,
        /// or a <see cref="BadRequestObjectResult"/> or <see cref="ConflictObjectResult"/> with an appropriate error.
        /// </returns>
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
