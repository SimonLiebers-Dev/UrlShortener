using Moq.Protected;
using Moq;
using System.Net;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Backend.Models;
using System.Text.Json;

namespace UrlShortener.Test.Backend.Unit.Business
{
    [TestFixture]
    public class UserAgentTests
    {
        [Test]
        public async Task GetUserAgentAsync_ReturnsValidResult_OnSuccess()
        {
            // Arrange
            var expectedResponse = new UserAgentApiResponse
            {
                BrowserFamily = "Chrome",
                OsFamily = "Windows"
            };

            var json = JsonSerializer.Serialize(expectedResponse);
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json)
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var service = new UserAgentService(httpClient);

            // Act
            var result = await service.GetUserAgentAsync("Mozilla/5.0");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result!.BrowserFamily, Is.EqualTo("Chrome"));
                Assert.That(result.OsFamily, Is.EqualTo("Windows"));
            });
        }

        [Test]
        public async Task GetUserAgentAsync_ReturnsNull_OnHttpError()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Simulated failure"));

            var httpClient = new HttpClient(mockHandler.Object);
            var service = new UserAgentService(httpClient);

            // Act
            var result = await service.GetUserAgentAsync("invalid");

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
