using Blazorise;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Moq;
using UrlShortener.App.Blazor.Client.Api;
using UrlShortener.App.Blazor.Client.Business;
using UrlShortener.App.Shared.Dto;

namespace UrlShortener.Test.Frontend.Unit.Business
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IAuthApi> _authApiMock;
        private Mock<INotificationService> _notificationServiceMock;
        private Mock<AuthenticationStateProvider> _authStateProviderMock;
        private Mock<NavigationManager> _navigationManagerMock;
        private AuthService _authService;

        [SetUp]
        public void SetUp()
        {
            _authApiMock = new Mock<IAuthApi>();
            _notificationServiceMock = new Mock<INotificationService>();
            _authStateProviderMock = new Mock<AuthenticationStateProvider>();
            _navigationManagerMock = new Mock<NavigationManager>();
            _authService = new AuthService(_authApiMock.Object, _notificationServiceMock.Object, _authStateProviderMock.Object, _navigationManagerMock.Object);
        }

        [Test]
        public async Task LoginAsync_InvalidCredentials_ShowsErrorNotification()
        {
            // Arrange
            _authApiMock.Setup(api => api.Login(It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync((string?)null);

            // Act
            await _authService.LoginAsync("user@example.com", "wrongpassword");

            // Assert
            _notificationServiceMock.Verify(ns => ns.Error("Email address or password wrong", It.IsAny<string>(), It.IsAny<Action<NotificationOptions>>()), Times.Once);
        }

        [Test]
        public async Task RegisterAsync_EmailAlreadyExists_ShowsErrorNotification()
        {
            // Arrange
            var response = new RegisterResponseDto { Success = false, ErrorType = RegisterErrorType.EmailAlreadyExists };
            _authApiMock.Setup(api => api.Register(It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(response);

            // Act
            await _authService.RegisterAsync("existing@example.com", "password");

            // Assert
            _notificationServiceMock.Verify(ns => ns.Error("Email already exists.", It.IsAny<string>(), It.IsAny<Action<NotificationOptions>>()), Times.Once);
        }

        [Test]
        public async Task RegisterAsync_MissingEmailOrPassword_ShowsErrorNotification()
        {
            // Arrange
            var response = new RegisterResponseDto { Success = false, ErrorType = RegisterErrorType.MissingEmailOrPassword };
            _authApiMock.Setup(api => api.Register(It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(response);

            // Act
            await _authService.RegisterAsync("", "");

            // Assert
            _notificationServiceMock.Verify(ns => ns.Error("Please provide email and password.", It.IsAny<string>(), It.IsAny<Action<NotificationOptions>>()), Times.Once);
        }
    }
}
