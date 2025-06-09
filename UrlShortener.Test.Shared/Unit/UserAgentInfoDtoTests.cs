using UrlShortener.App.Shared.Dto;

namespace UrlShortener.Test.Shared.Unit
{
    [TestFixture]
    public class UserAgentInfoDtoTests
    {
        [Test]
        public void Constructor_InitializesStringsWithEmpty_AndObjectsAsNull()
        {
            var dto = new UserAgentInfoDto();

            Assert.Multiple(() =>
            {
                Assert.That(dto.BrowserFamily, Is.EqualTo(string.Empty));
                Assert.That(dto.OsFamily, Is.EqualTo(string.Empty));
                Assert.That(dto.Client, Is.Null);
                Assert.That(dto.Device, Is.Null);
                Assert.That(dto.Os, Is.Null);
            });
        }

        [Test]
        public void Properties_CanBeSetAndRetrieved()
        {
            var client = new UserAgentClientDto
            {
                Name = "Firefox",
                Version = "99.0",
                Engine = "Gecko",
                EngineVersion = "20100101",
                Type = "Browser"
            };

            var device = new UserAgentDeviceDto
            {
                Brand = "Google",
                Model = "Pixel 7",
                Type = "Smartphone"
            };

            var os = new UserAgentOsDto
            {
                Name = "Android",
                Platform = "Linux",
                Version = "13"
            };

            var dto = new UserAgentInfoDto
            {
                BrowserFamily = "Firefox",
                Client = client,
                Device = device,
                Os = os,
                OsFamily = "Android"
            };

            Assert.Multiple(() =>
            {
                Assert.That(dto.BrowserFamily, Is.EqualTo("Firefox"));
                Assert.That(dto.Client, Is.EqualTo(client));
                Assert.That(dto.Device, Is.EqualTo(device));
                Assert.That(dto.Os, Is.EqualTo(os));
                Assert.That(dto.OsFamily, Is.EqualTo("Android"));
            });
        }
    }
}
