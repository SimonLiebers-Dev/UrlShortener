using Microsoft.AspNetCore.WebUtilities;
using Moq;
using System.Text;
using System.Text.Json;
using UrlShortener.App.Frontend.Business;

namespace UrlShortener.Test.Frontend.Unit.Business
{
    [TestFixture]
    public class AppAuthenticationStateProviderTests
    {
        private Mock<ILocalStorageService> _localStorageMock;
        private HttpClient _httpClient;
        private AppAuthenticationStateProvider _authProvider;

        [SetUp]
        public void SetUp()
        {
            _localStorageMock = new Mock<ILocalStorageService>();
            _httpClient = new HttpClient();
            _authProvider = new AppAuthenticationStateProvider(_localStorageMock.Object, _httpClient);
        }

        [Test]
        public async Task GetAuthenticationStateAsync_NoToken_ReturnsUnauthenticatedUser()
        {
            // Arrange
            _localStorageMock.Setup(x => x.GetItemAsync("authToken"))
                             .ReturnsAsync((string?)null);

            // Act
            var authState = await _authProvider.GetAuthenticationStateAsync();
            var identity = authState.User.Identity;

            // Assert
            Assert.That(identity, Is.Not.Null);
            Assert.That(identity.IsAuthenticated, Is.False);
        }

        [Test]
        public async Task GetAuthenticationStateAsync_WithToken_ReturnsAuthenticatedUser()
        {
            // Arrange
            var token = CreateTestToken();
            _localStorageMock.Setup(x => x.GetItemAsync("authToken"))
                             .ReturnsAsync(token);

            // Act
            var authState = await _authProvider.GetAuthenticationStateAsync();
            var identity = authState.User.Identity;

            // Assert
            Assert.That(identity, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(identity.IsAuthenticated, Is.True);
                Assert.That(authState.User.FindFirst("Name")?.Value, Is.EqualTo("test@gmail.com"));
                Assert.That(_httpClient.DefaultRequestHeaders.Authorization?.Scheme, Is.EqualTo("Bearer"));
                Assert.That(_httpClient.DefaultRequestHeaders.Authorization?.Parameter, Is.EqualTo(token));
            });
        }

        [Test]
        public async Task TriggerLoginAsync_SavesTokenAndNotifies()
        {
            // Arrange
            var token = "testToken";
            var eventCalled = false;
            _authProvider.AuthenticationStateChanged += _ => eventCalled = true;

            // Act
            await _authProvider.TriggerLoginAsync(token);

            // Assert
            _localStorageMock.Verify(x => x.SetItemAsync("authToken", token), Times.Once);
            Assert.That(eventCalled, Is.True);
        }

        [Test]
        public async Task TriggerLogoutAsync_RemovesTokenAndNotifies()
        {
            // Arrange
            var eventCalled = false;
            _authProvider.AuthenticationStateChanged += _ => eventCalled = true;

            // Act
            await _authProvider.TriggerLogoutAsync();

            // Assert
            _localStorageMock.Verify(x => x.RemoveItemAsync("authToken"), Times.Once);
            Assert.That(eventCalled, Is.True);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }

        private static string CreateTestToken()
        {
            var claims = new { Name = "test@gmail.com" };
            var payload = JsonSerializer.Serialize(claims);
            var base64Payload = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(payload));
            return $"header.{base64Payload}.signature";
        }
    }
}
