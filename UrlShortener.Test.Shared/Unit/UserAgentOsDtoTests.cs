using UrlShortener.App.Shared.Dto;

namespace UrlShortener.Test.Shared.Unit
{
    [TestFixture]
    public class UserAgentOsDtoTests
    {
        [Test]
        public void Constructor_InitializesWithEmptyStrings()
        {
            var dto = new UserAgentOsDto();

            Assert.Multiple(() =>
            {
                Assert.That(dto.Name, Is.EqualTo(string.Empty));
                Assert.That(dto.Platform, Is.EqualTo(string.Empty));
                Assert.That(dto.Version, Is.EqualTo(string.Empty));
            });
        }

        [Test]
        public void Properties_CanBeSetAndRetrieved()
        {
            var dto = new UserAgentOsDto
            {
                Name = "iOS",
                Platform = "ARM64",
                Version = "16.5.1"
            };

            Assert.Multiple(() =>
            {
                Assert.That(dto.Name, Is.EqualTo("iOS"));
                Assert.That(dto.Platform, Is.EqualTo("ARM64"));
                Assert.That(dto.Version, Is.EqualTo("16.5.1"));
            });
        }
    }
}
