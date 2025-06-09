using UrlShortener.App.Shared.Dto;

namespace UrlShortener.Test.Shared.Unit
{
    [TestFixture]
    public class UserAgentDeviceDtoTests
    {
        [Test]
        public void Constructor_InitializesWithEmptyStrings()
        {
            var dto = new UserAgentDeviceDto();

            Assert.Multiple(() =>
            {
                Assert.That(dto.Brand, Is.EqualTo(string.Empty));
                Assert.That(dto.Model, Is.EqualTo(string.Empty));
                Assert.That(dto.Type, Is.EqualTo(string.Empty));
            });
        }

        [Test]
        public void Properties_CanBeSetAndRetrieved()
        {
            var dto = new UserAgentDeviceDto
            {
                Brand = "Apple",
                Model = "iPhone 14 Pro",
                Type = "Smartphone"
            };

            Assert.Multiple(() =>
            {
                Assert.That(dto.Brand, Is.EqualTo("Apple"));
                Assert.That(dto.Model, Is.EqualTo("iPhone 14 Pro"));
                Assert.That(dto.Type, Is.EqualTo("Smartphone"));
            });
        }
    }
}
