using Blazorise;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Moq;
using UrlShortener.App.Blazor.Client.Business;
using TestContext = Bunit.TestContext;
using UrlShortener.App.Blazor.Client.Components.Form;
using Blazorise.Tailwind;
using Blazorise.Icons.FontAwesome;

namespace UrlShortener.Test.Frontend.Unit.Components.Form
{
    [TestFixture]
    public class LoginFormTests : TestContext
    {
        private Mock<IAuthService> _authServiceMock;
        private Mock<INotificationService> _notificationServiceMock;
        private Mock<AuthenticationStateProvider> _authStateProviderMock;
        private Mock<NavigationManager> _navigationManagerMock;

        [OneTimeSetUp]
        public void Setup()
        {
            Services
                .AddBlazorise(options =>
                {
                    options.Immediate = true;
                })
                .AddTailwindProviders()
                .AddFontAwesomeIcons();

            _authServiceMock = new Mock<IAuthService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _authStateProviderMock = new Mock<AuthenticationStateProvider>();
            _navigationManagerMock = new Mock<NavigationManager>();

            Services.AddSingleton(_authServiceMock.Object);
            Services.AddSingleton(_notificationServiceMock.Object);
            Services.AddSingleton(_authStateProviderMock.Object);
            Services.AddSingleton(_navigationManagerMock.Object);
        }

        [Test]
        public async Task LoginUser_WhenValidInput_ShouldCallAuthService()
        {
            // Arrange
            var component = RenderComponent<LoginForm>();
            var emailInput = component.Find("#login-email-input");
            var passwordInput = component.Find("#login-password-input");
            var loginButton = component.Find("#login-btn");

            emailInput.Input("test@example.com");
            passwordInput.Input("SecurePassword123");

            // Act
            await loginButton.ClickAsync(new());

            // Assert
            _authServiceMock.Verify(x => x.LoginAsync("test@example.com", "SecurePassword123"), Times.Once);
        }

        [Test]
        public async Task LoginUser_WhenInvalidEmail_ShouldNotCallAuthService()
        {
            // Arrange
            var component = RenderComponent<LoginForm>();
            var emailInput = component.Find("#login-email-input");
            var passwordInput = component.Find("#login-password-input");
            var loginButton = component.Find("#login-btn");

            emailInput.Input("InvalidEmail");
            passwordInput.Input("SecurePassword123");

            // Act
            await loginButton.ClickAsync(new());

            // Assert
            _authServiceMock.Verify(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task LoginUser_WhenEmptyForm_ShouldNotCallAuthService()
        {
            // Arrange
            var component = RenderComponent<LoginForm>();
            var loginButton = component.Find("#login-btn");

            // Act
            await loginButton.ClickAsync(new());

            // Assert
            _authServiceMock.Verify(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
