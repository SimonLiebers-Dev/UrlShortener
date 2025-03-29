using Moq;
using System.Net;
using UrlShortener.App.Blazor.Client.Api;
using UrlShortener.App.Shared.Dto;
using UrlShortener.Test.Frontend.Extensions;

namespace UrlShortener.Test.Frontend.Unit.Api
{
    [TestFixture]
    public class AuthApiTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private AuthApi _authApi;

        [SetUp]
        public void SetUp()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://example.com/")
            };
            _authApi = new AuthApi(_httpClient);
        }

        [Test]
        public async Task Login_Success_ReturnsToken()
        {
            // Arrange
            var expectedToken = "test-token";
            var expectedResponse = new LoginResponseDto()
            {
                Token = expectedToken
            };
            _httpMessageHandlerMock.SetupRequest(HttpMethod.Post, "api/auth/login", expectedResponse);

            // Act
            var result = await _authApi.Login("test@example.com", "password");

            // Assert
            Assert.That(result, Is.EqualTo(expectedToken));
        }

        [Test]
        public async Task Login_Failure_ReturnsNull()
        {
            // Arrange
            _httpMessageHandlerMock.SetupRequest(HttpMethod.Post, "api/auth/login", HttpStatusCode.Unauthorized);

            // Act
            var result = await _authApi.Login("test@example.com", "wrongpassword");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Register_Success_ReturnsResponse()
        {
            // Arrange
            var expectedResponse = new RegisterResponseDto { Success = true };
            _httpMessageHandlerMock.SetupRequest(HttpMethod.Post, "api/auth/register", expectedResponse);

            // Act
            var result = await _authApi.Register("test@example.com", "password");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Success, Is.True);
        }

        [Test]
        public async Task Register_Failure_ReturnsNull()
        {
            // Arrange
            _httpMessageHandlerMock.SetupRequest(HttpMethod.Post, "api/auth/register", HttpStatusCode.BadRequest);

            // Act
            var result = await _authApi.Register("test@example.com", "password");

            // Assert
            Assert.That(result, Is.Null);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}
