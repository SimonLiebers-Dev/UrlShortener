using Blazorise;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Moq;
using UrlShortener.App.Blazor.Client.Business;
using TestContext = Bunit.TestContext;
using UrlShortener.App.Blazor.Client.Components.Form;
using Blazorise.Tailwind;
using Blazorise.Icons.FontAwesome;
using Microsoft.Win32;

namespace UrlShortener.Test.Frontend.Unit.Components.Form
{
    [TestFixture]
    public class RegisterFormTests : TestContext
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
        public async Task RegisterUser_WhenValidInput_ShouldCallAuthService()
        {
            // Arrange
            var component = RenderComponent<RegisterForm>();
            var emailInput = component.Find("#register-email-input");
            var passwordInput1 = component.Find("#register-password-input-1");
            var passwordInput2 = component.Find("#register-password-input-2");
            var registerButton = component.Find("#register-btn");

            emailInput.Input("test@example.com");
            passwordInput1.Input("SecurePassword123");
            passwordInput2.Input("SecurePassword123");

            // Act
            await registerButton.ClickAsync(new());

            // Assert
            _authServiceMock.Verify(x => x.RegisterAsync("test@example.com", "SecurePassword123"), Times.Once);
        }

        [Test]
        public async Task RegisterUser_WhenInvalidEmail_ShouldNotCallAuthService()
        {
            // Arrange
            var component = RenderComponent<RegisterForm>();
            var emailInput = component.Find("#register-email-input");
            var passwordInput1 = component.Find("#register-password-input-1");
            var passwordInput2 = component.Find("#register-password-input-2");
            var registerButton = component.Find("#register-btn");

            emailInput.Input("NoValidEmail");
            passwordInput1.Input("SecurePassword123");
            passwordInput2.Input("SecurePassword123");

            // Act
            await registerButton.ClickAsync(new());

            // Assert
            _authServiceMock.Verify(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task RegisterUser_WhenPasswordsNotMatching_ShouldNotCallAuthService()
        {
            // Arrange
            var component = RenderComponent<RegisterForm>();
            var emailInput = component.Find("#register-email-input");
            var passwordInput1 = component.Find("#register-password-input-1");
            var passwordInput2 = component.Find("#register-password-input-2");
            var registerButton = component.Find("#register-btn");

            emailInput.Input("test@example.com");
            passwordInput1.Input("SecurePassword1");
            passwordInput2.Input("SecurePassword2");

            // Act
            await registerButton.ClickAsync(new());

            // Assert
            _authServiceMock.Verify(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task RegisterUser_WhenEmptyForm_ShouldNotCallAuthService()
        {
            // Arrange
            var component = RenderComponent<RegisterForm>();
            var registerButton = component.Find("#register-btn");

            // Act
            await registerButton.ClickAsync(new());

            // Assert
            _authServiceMock.Verify(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
