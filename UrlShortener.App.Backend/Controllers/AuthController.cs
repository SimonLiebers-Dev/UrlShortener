using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
    [EnableRateLimiting("fixed")]
    public class AuthController(ILogger<AuthController> Logger, IJwtTokenGenerator JwtTokenGenerator, AppDbContext DbContext) : ControllerBase
    {
        /// <summary>
        /// Maximum length for email addresses, as per RFC 5321 and RFC 5322 standards.
        /// </summary>
        private const int MAX_EMAIL_LEN = 254;

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
            Logger.LogInformation("Login(email={Email})", request.Email);

            // Get user by email
            var user = DbContext.Users.SingleOrDefault(u => u.Email == request.Email);

            // Check if user exists
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            // Verify password
            string hashedPassword = PasswordUtils.HashPassword(request.Password, user.Salt);
            if (hashedPassword != user.PasswordHash)
            {
                return Unauthorized("Invalid email or password");
            }

            // Generate JWT token
            var token = JwtTokenGenerator.GenerateToken(user.Email);

            Logger.LogInformation("Login(email={Email}) - User authenticated successfully", request.Email);

            // Return success response with token
            return Ok(new LoginResponseDto { Token = token });
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
            Logger.LogInformation("Register(email={Tenant})", request.Email);

            // Check if mail and password set
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new RegisterResponseDto { Success = false, ErrorType = RegisterErrorType.MissingEmailOrPassword });
            }

            // Check if password length is long enough
            if (request.Password.Length < 6)
            {
                return BadRequest(new RegisterResponseDto { Success = false, ErrorType = RegisterErrorType.PasswordPolicyViolation });
            }

            // Validate email format
            var email = new EmailAddressAttribute();
            if (request.Email.Any(char.IsWhiteSpace) || request.Email.Length > MAX_EMAIL_LEN || !email.IsValid(request.Email))
            {
                return BadRequest(new RegisterResponseDto { Success = false, ErrorType = RegisterErrorType.MissingEmailOrPassword });
            }

            // Check if email already exists
            if (await DbContext.Users.AnyAsync(u => u.Email == request.Email))
            {
                return Conflict(new RegisterResponseDto { Success = false, ErrorType = RegisterErrorType.EmailAlreadyExists });
            }

            // Generate salt and password hash
            string salt = PasswordUtils.GenerateSalt();
            string hashedPassword = PasswordUtils.HashPassword(request.Password, salt);

            // Create new user entity
            var newUser = new User
            {
                Email = request.Email,
                PasswordHash = hashedPassword,
                Salt = salt
            };

            // Add user to the database
            DbContext.Users.Add(newUser);
            await DbContext.SaveChangesAsync();

            Logger.LogInformation("Register(email={Email}) - User registered successfully", request.Email);

            // Return success response
            return Ok(new RegisterResponseDto());
        }
    }
}
