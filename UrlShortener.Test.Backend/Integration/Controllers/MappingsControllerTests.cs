using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using UrlShortener.App.Backend;
using UrlShortener.App.Backend.Utils;
using UrlShortener.App.Shared.Dto;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.Test.Backend.Integration.Controllers
{
    [TestFixture]
    public class MappingsControllerTests
    {
        private HttpClient _httpClient;
        private WebApplicationFactory<Program> _webApplicationFactory;

        [SetUp]
        public void Setup()
        {
            _webApplicationFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
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
                        Email = "test@test.com",
                        PasswordHash = PasswordUtils.HashPassword("TestPassword", passwordSalt),
                        Salt = passwordSalt
                    };
                    db.Users.Add(testUser);

                    var urlMapping = new UrlMapping
                    {
                        LongUrl = "LongUrl",
                        Path = "Path",
                        CreatedAt = DateTime.UtcNow,
                        User = "test@test.com",
                        Name = "TestMapping"
                    };
                    db.UrlMappings.Add(urlMapping);

                    db.SaveChanges();
                });
            });

            _httpClient = _webApplicationFactory.CreateClient();
        }

        [Test]
        public async Task CreateMapping_ValidData_ReturnsOkWithShortUrl()
        {
            // Arrange
            var loginResponse = await LoginAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse?.Token);

            var createMappingRequest = new CreateMappingRequestDto
            {
                LongUrl = "https://example.com",
                Name = "Example"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("api/mappings/create", createMappingRequest);
            var result = await response.Content.ReadFromJsonAsync<CreateMappingResponseDto>();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ShortUrl, Is.Not.Empty);
        }

        [Test]
        public async Task CreateMapping_MissingUrl_ReturnsBadRequest()
        {
            // Arrange
            var loginResponse = await LoginAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse?.Token);

            var createMappingRequest = new CreateMappingRequestDto
            {
                LongUrl = "",
                Name = "Example"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("api/mappings/create", createMappingRequest);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task CreateMapping_MissingName_ReturnsBadRequest()
        {
            // Arrange
            var loginResponse = await LoginAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse?.Token);

            var createMappingRequest = new CreateMappingRequestDto
            {
                LongUrl = "https://test.com",
                Name = ""
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("api/mappings/create", createMappingRequest);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task CreateMapping_Unauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var createMappingRequest = new CreateMappingRequestDto
            {
                LongUrl = "https://test.com",
                Name = "Test"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("api/mappings/create", createMappingRequest);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GetMappings_ValidUser_ReturnsOkWithMappings()
        {
            // Arrange
            var loginResponse = await LoginAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse?.Token);

            // Act
            var response = await _httpClient.GetAsync("api/mappings/all");
            var result = await response.Content.ReadFromJsonAsync<List<UrlMappingDto>>();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task DeleteMapping_ValidMappingId_ReturnsOk()
        {
            // Arrange
            var loginResponse = await LoginAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse?.Token);

            await CreateMapping();

            var mappingsResponse = await _httpClient.GetAsync("api/mappings/all");
            var mappingsResult = await mappingsResponse.Content.ReadFromJsonAsync<List<UrlMappingDto>>();

            // Assert
            Assert.That(mappingsResult, Has.Count.GreaterThan(0));

            // Act
            var response = await _httpClient.DeleteAsync($"api/mappings/{mappingsResult.First().Id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task DeleteMapping_FailedDeletion_ReturnsBadRequest()
        {
            // Arrange
            var loginResponse = await LoginAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse?.Token);

            // Act
            var response = await _httpClient.DeleteAsync($"api/mappings/473894");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task DeleteMapping_Unauthorized_ReturnsUnauthorized()
        {
            // Act
            var response = await _httpClient.DeleteAsync($"api/mappings/473894");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GetStats_ValidUser_ReturnsOkWithStats()
        {
            // Arrange
            var loginResponse = await LoginAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse?.Token);

            // Act
            var response = await _httpClient.GetAsync("api/mappings/stats");
            var result = await response.Content.ReadFromJsonAsync<UserStatsDto>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetStats_Unauthorized_ReturnsUnauthorized()
        {
            // Act
            var response = await _httpClient.GetAsync("api/mappings/stats");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        private async Task CreateMapping()
        {
            var createMappingRequest = new CreateMappingRequestDto
            {
                LongUrl = "https://example.com",
                Name = "Example"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("api/mappings/create", createMappingRequest);
            await response.Content.ReadFromJsonAsync<CreateMappingResponseDto>();
        }

        [TearDown]
        public void TearDown()
        {
            _webApplicationFactory.Dispose();
            _httpClient.Dispose();
        }

        private async Task<LoginResponseDto?> LoginAsync(string email = "test@test.com", string password = "TestPassword")
        {
            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginRequest);
            return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        }
    }
}
