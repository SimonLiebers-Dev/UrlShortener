using Blazorise;
using Blazorise.Icons.FontAwesome;
using Blazorise.Tailwind;
using Microsoft.AspNetCore.Components;
using Moq;
using UrlShortener.App.Blazor.Client.Business;
using UrlShortener.App.Blazor.Client.Components.Form;
using UrlShortener.App.Blazor.Client.Components.Pages;
using TestContext = Bunit.TestContext;

namespace UrlShortener.Test.Frontend.Unit.Components.Pages
{
    [TestFixture]
    public class AuthPageTests : TestContext
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            var mockAuthService = new Mock<IAuthService>();

            Services
                .AddBlazorise(options =>
                {
                    options.Immediate = true;
                })
                .AddTailwindProviders()
                .AddFontAwesomeIcons();

            Services.AddSingleton(mockAuthService.Object);
        }

        [Test]
        public void RendersLoginComponentWhenAuthTypeIsLogin()
        {
            // Arrange
            var navigationManager = Services.GetRequiredService<NavigationManager>();
            var component = RenderComponent<AuthPage>();

            // Act
            var uri = navigationManager.GetUriWithQueryParameter("type", "login");
            navigationManager.NavigateTo(uri);
            var loginForm = component.FindComponent<LoginForm>();

            // Assert
            Assert.That(loginForm, Is.Not.Null);
        }

        [Test]
        public void RendersRegisterComponentWhenAuthTypeIsRegister()
        {
            // Arrange
            var navigationManager = Services.GetRequiredService<NavigationManager>();
            var component = RenderComponent<AuthPage>();

            // Act
            var uri = navigationManager.GetUriWithQueryParameter("type", "register");
            navigationManager.NavigateTo(uri);
            var registerForm = component.FindComponent<RegisterForm>();

            // Assert
            Assert.That(registerForm, Is.Not.Null);
        }

        [Test]
        public void NavigatesToDefaultWhenInvalidAuthTypeIsProvided()
        {
            // Arrange
            var navigationManager = Services.GetRequiredService<NavigationManager>();
            var component = RenderComponent<AuthPage>();

            // Act 
            var uri = navigationManager.GetUriWithQueryParameter("type", "invalidType");
            navigationManager.NavigateTo(uri);

            // Assert
            Assert.That(navigationManager.Uri.Contains("type=login"));
        }
    }
}
