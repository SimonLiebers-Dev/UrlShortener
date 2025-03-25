using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using UrlShortener.App.Backend;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.App.Shared.Models;
using UrlShortener.App.Backend.Utils;
using UrlShortener.App.Shared.Dto;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace UrlShortener.Test.Backend.Integration.Controllers
{
    [TestFixture]
    public class AuthControllerTests : WebApplicationFactory<Program>
    {
        private HttpClient _httpClient;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddEntityFrameworkInMemoryDatabase();

                var provider = services
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();

                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();

                db.Database.EnsureCreated();

                var passwordSalt = PasswordUtils.GenerateSalt();
                var testUser = new User()
                {
                    Email = "test@gmail.com",
                    PasswordHash = PasswordUtils.HashPassword("TestPassword", passwordSalt),
                    Salt = passwordSalt
                };

                db.Users.Add(testUser);
                db.SaveChanges();
            });
        }

        [SetUp]
        public void Setup()
        {
            _httpClient = CreateClient();
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@gmail.com",
                Password = "TestPassword"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginRequest);
            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Token, Is.Not.Empty);
        }

        [Test]
        public async Task Login_InvalidEmail_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "nonexistentuser@example.com",
                Password = "WrongPassword"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginRequest);

            // Assert
            Assert.That(response.IsSuccessStatusCode, Is.False);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Register_ValidData_ReturnsOk()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "newuser@example.com",
                Password = "NewUserPassword"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", registerRequest);
            var result = await response.Content.ReadFromJsonAsync<RegisterResponseDto>();

            // Assert
            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
        }

        [Test]
        public async Task Register_MissingEmail_ReturnsError()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "",
                Password = "NewUserPassword"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", registerRequest);
            var result = await response.Content.ReadFromJsonAsync<RegisterResponseDto>();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorType, Is.EqualTo(RegisterErrorType.MissingEmailOrPassword));
        }

        [Test]
        public async Task Register_EmailAlreadyExists_ReturnsError()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "test@gmail.com",
                Password = "TestPassword"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", registerRequest);
            var result = await response.Content.ReadFromJsonAsync<RegisterResponseDto>();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorType, Is.EqualTo(RegisterErrorType.EmailAlreadyExists));
        }
    }
}
