using UrlShortener.App.Shared.Dto;

namespace UrlShortener.Test.Shared.Unit
{
    [TestFixture]
    public class UserAgentClientDtoTests
    {
        [Test]
        public void Constructor_InitializesWithEmptyStrings()
        {
            var dto = new UserAgentClientDto();

            Assert.Multiple(() =>
            {
                Assert.That(dto.Engine, Is.EqualTo(string.Empty));
                Assert.That(dto.EngineVersion, Is.EqualTo(string.Empty));
                Assert.That(dto.Name, Is.EqualTo(string.Empty));
                Assert.That(dto.Type, Is.EqualTo(string.Empty));
                Assert.That(dto.Version, Is.EqualTo(string.Empty));
            });
        }

        [Test]
        public void Properties_CanBeSetAndRetrieved()
        {
            var dto = new UserAgentClientDto
            {
                Engine = "Blink",
                EngineVersion = "123.4",
                Name = "Chrome",
                Type = "Browser",
                Version = "120.0.1"
            };

            Assert.Multiple(() =>
            {
                Assert.That(dto.Engine, Is.EqualTo("Blink"));
                Assert.That(dto.EngineVersion, Is.EqualTo("123.4"));
                Assert.That(dto.Name, Is.EqualTo("Chrome"));
                Assert.That(dto.Type, Is.EqualTo("Browser"));
                Assert.That(dto.Version, Is.EqualTo("120.0.1"));
            });
        }
    }
}
