using Microsoft.JSInterop;
using Moq;
using UrlShortener.App.Frontend.Business;

namespace UrlShortener.Test.Frontend.Unit.Business
{
    [TestFixture]
    public class LocalStorageServiceTests
    {
        private Mock<IJSRuntime> _jsRuntimeMock;
        private LocalStorageService _localStorageService;

        [SetUp]
        public void SetUp()
        {
            _jsRuntimeMock = new Mock<IJSRuntime>();
            _localStorageService = new LocalStorageService(_jsRuntimeMock.Object);
        }

        [Test]
        public async Task GetItemAsync_ItemExists_ReturnsValue()
        {
            // Arrange
            _jsRuntimeMock
                .Setup(js => js.InvokeAsync<string>("localStorageInterop.getItem", It.IsAny<object[]>()))
                .ReturnsAsync("storedValue");

            // Act
            var result = await _localStorageService.GetItemAsync("myKey");

            // Assert
            Assert.That(result, Is.EqualTo("storedValue"));
        }

        [Test]
        public async Task GetItemAsync_ItemDoesNotExist_ReturnsNull()
        {
            // Arrange
            _jsRuntimeMock
                .Setup(js => js.InvokeAsync<string>("localStorageInterop.getItem", It.IsAny<object[]>()))
                .ReturnsAsync(string.Empty);

            // Act
            var result = await _localStorageService.GetItemAsync("nonExistingKey");

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
