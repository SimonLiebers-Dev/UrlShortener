using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using UrlShortener.App.Frontend.Business;
using UrlShortener.App.Shared.Dto;

namespace UrlShortener.Test.Frontend.Unit.Business
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private AuthService _authService;

        [SetUp]
        public void SetUp()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://testapi.com/")
            };

            _authService = new AuthService(_httpClient);
        }

        [Test]
        public async Task Login_UnsuccessfulResponse_ReturnsNull()
        {
            // Arrange
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized
                });

            // Act
            var result = await _authService.Login("test@example.com", "wrongpassword");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Login_SuccessfulResponse_ReturnsToken()
        {
            // Arrange
            var fakeResponse = new LoginResponseDto { Token = "fake-jwt-token" };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(fakeResponse)
                });

            // Act
            var result = await _authService.Login("test@example.com", "correctpassword");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo("fake-jwt-token"));
        }

        [Test]
        public async Task Register_UnsuccessfulResponse_ReturnsNull()
        {
            // Arrange
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            // Act
            var result = await _authService.Register("test@example.com", "weakpassword");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Register_SuccessfulResponse_ReturnsRegisterResponseDto()
        {
            // Arrange
            var fakeResponse = new RegisterResponseDto { Success = true, ErrorType = RegisterErrorType.None };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(fakeResponse)
                });

            // Act
            var result = await _authService.Register("test@example.com", "StrongPassword123");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.ErrorType, Is.EqualTo(RegisterErrorType.None));
            });
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}
