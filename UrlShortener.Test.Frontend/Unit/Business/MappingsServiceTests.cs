using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Moq;
using UrlShortener.App.Frontend.Business;
using UrlShortener.App.Shared.Dto;
using UrlShortener.Test.Frontend.Extensions;
using Moq.Protected;
using System.Net;

namespace UrlShortener.Test.Frontend.Unit.Business
{
    [TestFixture]
    public class MappingsServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private Mock<AuthenticationStateProvider> _authStateProviderMock;
        private Mock<NavigationManager> _navigationManagerMock;
        private MappingsService _mappingsService;

        [SetUp]
        public void SetUp()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpMessageHandlerMock.Protected()
                .Setup("Dispose", ItExpr.IsAny<bool>())
                .Verifiable();

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://test.com/")
            };

            _authStateProviderMock = new Mock<AuthenticationStateProvider>();
            _navigationManagerMock = new Mock<NavigationManager>();

            _mappingsService = new MappingsService(_httpClient, _authStateProviderMock.Object, _navigationManagerMock.Object);
        }

        [Test]
        public async Task GetMappings_Success_ReturnsMappings()
        {
            // Arrange
            var expectedMappings = new List<UrlMappingDto> {
                new() {  Id = 1, User = "test@gmail.com" }
            };
            _httpMessageHandlerMock.SetupRequest(HttpMethod.Get, "api/mappings/all", expectedMappings);

            // Act
            var result = await _mappingsService.GetMappings();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(expectedMappings.Count));
            Assert.Multiple(() =>
            {
                Assert.That(result[0].Id, Is.EqualTo(expectedMappings[0].Id));
                Assert.That(result[0].User, Is.EqualTo(expectedMappings[0].User));
            });
        }

        [Test]
        public async Task GetMappings_Failure_ReturnsNull()
        {
            // Arrange
            _httpMessageHandlerMock.SetupRequest(HttpMethod.Get, "api/mappings/all", HttpStatusCode.BadRequest);

            // Act
            var result = await _mappingsService.GetMappings();

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateMapping_Success_ReturnsResponse()
        {
            // Arrange
            var responseDto = new CreateMappingResponseDto { ShortUrl = "shortUrl" };
            _httpMessageHandlerMock.SetupRequest(HttpMethod.Post, "api/mappings/create", responseDto);

            // Act
            var result = await _mappingsService.CreateMapping("longUrl");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ShortUrl, Is.EqualTo(responseDto.ShortUrl));
        }

        [Test]
        public async Task CreateMapping_Failure_ReturnsNull()
        {
            // Arrange
            _httpMessageHandlerMock.SetupRequest(HttpMethod.Post, "api/mappings/create", HttpStatusCode.BadRequest);

            // Act
            var result = await _mappingsService.CreateMapping("https://example.com");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetStats_Success_ReturnsStats()
        {
            // Arrange
            var expectedStats = new UserStatsDto { Clicks = 42, Mappings = 1 };
            _httpMessageHandlerMock.SetupRequest(HttpMethod.Get, "api/mappings/stats", expectedStats);

            // Act
            var result = await _mappingsService.GetStats();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Clicks, Is.EqualTo(expectedStats.Clicks));
                Assert.That(result.Mappings, Is.EqualTo(expectedStats.Mappings));
            });
        }

        [Test]
        public async Task DeleteMapping_Success_ReturnsTrue()
        {
            // Arrange
            var mapping = new UrlMappingDto { Id = 1 };
            _httpMessageHandlerMock.SetupRequest(HttpMethod.Delete, $"api/mappings/{mapping.Id}", HttpStatusCode.OK);

            // Act
            var result = await _mappingsService.DeleteMapping(mapping);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeleteMapping_Failure_ReturnsFalse()
        {
            // Arrange
            var mapping = new UrlMappingDto { Id = 1 };
            _httpMessageHandlerMock.SetupRequest(HttpMethod.Delete, $"api/mappings/{mapping.Id}", HttpStatusCode.BadRequest);

            // Act
            var result = await _mappingsService.DeleteMapping(mapping);

            // Assert
            Assert.That(result, Is.False);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}
