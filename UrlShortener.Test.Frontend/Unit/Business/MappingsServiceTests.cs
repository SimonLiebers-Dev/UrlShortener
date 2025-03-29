using Microsoft.AspNetCore.Components;
using Moq;
using System.Net;
using UrlShortener.App.Blazor.Client.Business;
using UrlShortener.App.Shared.Dto;
using UrlShortener.Test.Frontend.Extensions;
using Microsoft.JSInterop;

namespace UrlShortener.Test.Frontend.Unit.Business
{
    [TestFixture]
    public class MappingsServiceTests
    {
        private Mock<IJSRuntime> _jsRuntimeMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private AppAuthenticationStateProvider _authStateProvider;
        private Mock<NavigationManager> _navigationManagerMock;
        private MappingsService _mappingsService;

        [SetUp]
        public void SetUp()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object) { BaseAddress = new Uri("https://example.com/") };
            _jsRuntimeMock = new Mock<IJSRuntime>();
            _authStateProvider = new AppAuthenticationStateProvider(_jsRuntimeMock.Object);
            _navigationManagerMock = new Mock<NavigationManager>();
            _navigationManagerMock = new Mock<NavigationManager>();
            _mappingsService = new MappingsService(_httpClient, _authStateProvider, _navigationManagerMock.Object);
        }

        [Test]
        public async Task GetMappings_Success_ReturnsMappings()
        {
            // Arrange
            var expectedMappings = new List<UrlMappingDto> { new() { Id = 1, LongUrl = "https://example.com" } };
            _httpMessageHandlerMock.SetupRequest(HttpMethod.Get, "api/mappings/all", expectedMappings);

            // Act
            var result = await _mappingsService.GetMappings();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].LongUrl, Is.EqualTo("https://example.com"));
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
            var expectedResponse = new CreateMappingResponseDto { ShortUrl = "https://short.url/abc" };
            _httpMessageHandlerMock.SetupRequest(HttpMethod.Post, "api/mappings/create", expectedResponse);

            // Act
            var result = await _mappingsService.CreateMapping("https://example.com");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ShortUrl, Is.EqualTo("https://short.url/abc"));
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
