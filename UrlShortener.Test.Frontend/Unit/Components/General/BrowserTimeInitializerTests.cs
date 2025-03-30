using Microsoft.JSInterop;
using Moq;
using UrlShortener.App.Blazor.Client.Business;
using UrlShortener.App.Blazor.Client.Components.General;
using TestContext = Bunit.TestContext;

namespace UrlShortener.Test.Frontend.Unit.Components.General
{
    [TestFixture]
    public class BrowserTimeInitializerTests : TestContext
    {
        private Mock<IJSRuntime> _jsRuntimeMock;
        private Mock<BrowserTimeProvider> _timeProviderMock;

        [OneTimeSetUp]
        public void Setup()
        {
            _jsRuntimeMock = new Mock<IJSRuntime>();
            _timeProviderMock = new Mock<BrowserTimeProvider>();

            Services.AddSingleton(_jsRuntimeMock.Object);
            Services.AddSingleton<TimeProvider>(_timeProviderMock.Object);
        }

        [Test]
        public void Component_WhenFirstRender_ShouldSetBrowserTimeZone()
        {
            // Arrange
            var expectedResult = "Europe/Berlin";
            _jsRuntimeMock.Setup(js => js.InvokeAsync<string?>("browserTimeZoneInterop.getBrowserTimeZone", It.IsAny<object[]>()))
                .ReturnsAsync(expectedResult);
            _timeProviderMock.Setup(tp => tp.SetBrowserTimeZone(expectedResult));

            // Act
            var component = RenderComponent<BrowserTimeInitializer>();

            // Assert
            _timeProviderMock.Verify(tp => tp.SetBrowserTimeZone(expectedResult), Times.Once);
        }
    }
}
