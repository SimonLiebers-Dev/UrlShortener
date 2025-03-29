using Blazorise;
using Blazorise.Icons.FontAwesome;
using Blazorise.Tailwind;
using Microsoft.AspNetCore.Components;
using Moq;
using UrlShortener.App.Blazor.Client.Business;
using UrlShortener.App.Blazor.Client.Components.Pages;
using TestContext = Bunit.TestContext;

namespace UrlShortener.Test.Frontend.Unit.Components.Pages
{
    [TestFixture]
    public class AuthTest : TestContext
    {
        [SetUp]
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
            var component = RenderComponent<Auth>();

            // Act
            var uri = navigationManager.GetUriWithQueryParameter("type", "login");
            navigationManager.NavigateTo(uri);
            var loginForm = component.FindComponent<Login>();

            // Assert
            Assert.That(loginForm, Is.Not.Null);
        }

        [Test]
        public void RendersRegisterComponentWhenAuthTypeIsRegister()
        {
            // Arrange
            var navigationManager = Services.GetRequiredService<NavigationManager>();
            var component = RenderComponent<Auth>();

            // Act
            var uri = navigationManager.GetUriWithQueryParameter("type", "register");
            navigationManager.NavigateTo(uri);
            var registerForm = component.FindComponent<Register>();

            // Assert
            Assert.That(registerForm, Is.Not.Null);
        }

        [Test]
        public void NavigatesToDefaultWhenInvalidAuthTypeIsProvided()
        {
            // Arrange
            var navigationManager = Services.GetRequiredService<NavigationManager>();
            var component = RenderComponent<Auth>();

            // Act 
            var uri = navigationManager.GetUriWithQueryParameter("type", "invalidType");
            navigationManager.NavigateTo(uri);

            // Assert
            Assert.That(navigationManager.Uri.Contains("type=login"));
        }
    }
}
