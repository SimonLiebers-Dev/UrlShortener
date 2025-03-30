using Blazorise;
using Blazorise.Icons.FontAwesome;
using Blazorise.Tailwind;
using Moq;
using UrlShortener.App.Blazor.Client.Components.General;
using UrlShortener.App.Shared.Dto;
using TestContext = Bunit.TestContext;

namespace UrlShortener.Test.Frontend.Unit.Components.General
{
    [TestFixture]
    public class MappingDetailsModalTests : TestContext
    {
        private Mock<TimeProvider> _timeProviderMock;

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

            _timeProviderMock = new Mock<TimeProvider>();

            Services.AddSingleton(_timeProviderMock.Object);
        }

        [Test]
        public void Modal_ShouldBeVisible_WhenUrlMappingIsSet()
        {
            // Arrange
            var urlMapping = new UrlMappingDto { Name = "Test Mapping", ShortUrl = "https://short.url" };
            var component = RenderComponent<MappingDetailsModal>(parameters => parameters.Add(p => p.UrlMapping, urlMapping));

            // Act
            var modal = component.Find("#mapping-details-modal-header");

            // Assert
            Assert.That(modal, Is.Not.Null);
        }

        [Test]
        public void Modal_ShouldNotBeVisible_WhenUrlMappingIsNotSet()
        {
            // Arrange
            var component = RenderComponent<MappingDetailsModal>(parameters => parameters.Add(p => p.UrlMapping, null));
            var modal = component.FindComponent<Modal>();

            // Assert
            Assert.That(modal.Instance.Visible, Is.False);
        }

        [Test]
        public async Task ModalVisibleChanged_WhenClosed_ShouldClearUrlMapping()
        {
            // Arrange
            var urlMapping = new UrlMappingDto { Name = "Test Mapping", ShortUrl = "https://short.url" };
            var component = RenderComponent<MappingDetailsModal>(parameters => parameters.Add(p => p.UrlMapping, urlMapping));
            var modal = component.FindComponent<Modal>();

            // Act
            var closeBtn = component.Find("#mapping-details-modal-btn-close");
            await closeBtn.ClickAsync(new());

            // Assert
            Assert.That(modal.Instance.Visible, Is.False);
        }
    }
}
