using Microsoft.JSInterop;
using Moq;
using Microsoft.JSInterop.Infrastructure;
using UrlShortener.App.Blazor.Client.Business;
using UrlShortener.Test.Frontend.Utils;

namespace UrlShortener.Test.Frontend.Unit.Business
{
    [TestFixture]
    public class AppAuthenticationStateProviderTests
    {
        private Mock<IJSRuntime> _jsRuntimeMock;
        private AppAuthenticationStateProvider _authStateProvider;

        [SetUp]
        public void SetUp()
        {
            _jsRuntimeMock = new Mock<IJSRuntime>();
            _authStateProvider = new AppAuthenticationStateProvider(_jsRuntimeMock.Object);
        }

        [Test]
        public async Task GetAuthenticationStateAsync_NoToken_ReturnsUnauthenticated()
        {
            // Act
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var identity = state.User.Identity;

            // Assert
            Assert.That(identity, Is.Null);
        }

        [Test]
        public async Task TryMarkUserAsAuthenticated_ValidToken_SetsAuthenticationState()
        {
            // Arrange
            var token = JwtUtils.GenerateTestToken("test@gmail.com");
            _jsRuntimeMock.Setup(js => js.InvokeAsync<IJSVoidResult?>("sessionStorage.setItem", It.IsAny<object[]>()));

            // Act
            await _authStateProvider.TryMarkUserAsAuthenticated(token);
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var identity = state.User.Identity;

            // Assert
            Assert.That(identity, Is.Not.Null);
            Assert.That(identity.IsAuthenticated, Is.True);
        }

        [Test]
        public async Task MarkUserAsLoggedOut_ClearsAuthenticationState()
        {
            // Arrange
            _jsRuntimeMock.Setup(js => js.InvokeAsync<IJSVoidResult?>("sessionStorage.removeItem", It.IsAny<object[]>()));

            // Act
            await _authStateProvider.MarkUserAsLoggedOut();
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var identity = state.User.Identity;

            // Assert
            Assert.That(identity, Is.Null);
        }
    }
}
