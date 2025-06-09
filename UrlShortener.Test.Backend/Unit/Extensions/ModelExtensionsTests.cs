using UrlShortener.App.Backend.Extensions;
using UrlShortener.App.Backend.Models;

namespace UrlShortener.Test.Backend.Unit.Extensions
{
    [TestFixture]
    public class ModelExtensionsTests
    {
        [Test]
        public void ToDto_UserAgentClient_MapsCorrectly()
        {
            var model = new UserAgentClient
            {
                Engine = "Blink",
                EngineVersion = "123",
                Name = "Chrome",
                Type = "Browser",
                Version = "120.0"
            };

            var dto = model.ToDto();

            Assert.Multiple(() =>
            {
                Assert.That(dto.Engine, Is.EqualTo(model.Engine));
                Assert.That(dto.EngineVersion, Is.EqualTo(model.EngineVersion));
                Assert.That(dto.Name, Is.EqualTo(model.Name));
                Assert.That(dto.Type, Is.EqualTo(model.Type));
                Assert.That(dto.Version, Is.EqualTo(model.Version));
            });
        }

        [Test]
        public void ToDto_UserAgentDevice_MapsCorrectly()
        {
            var model = new UserAgentDevice
            {
                Brand = "Samsung",
                Model = "Galaxy S21",
                Type = "Mobile"
            };

            var dto = model.ToDto();

            Assert.Multiple(() =>
            {
                Assert.That(dto.Brand, Is.EqualTo(model.Brand));
                Assert.That(dto.Model, Is.EqualTo(model.Model));
                Assert.That(dto.Type, Is.EqualTo(model.Type));
            });
        }

        [Test]
        public void ToDto_UserAgentOs_MapsCorrectly()
        {
            var model = new UserAgentOs
            {
                Name = "Android",
                Platform = "Linux",
                Version = "12"
            };

            var dto = model.ToDto();

            Assert.Multiple(() =>
            {
                Assert.That(dto.Name, Is.EqualTo(model.Name));
                Assert.That(dto.Platform, Is.EqualTo(model.Platform));
                Assert.That(dto.Version, Is.EqualTo(model.Version));
            });
        }

        [Test]
        public void ToDto_UserAgentApiResponse_MapsAllSubObjectsCorrectly()
        {
            var model = new UserAgentApiResponse
            {
                BrowserFamily = "Chrome",
                Client = new UserAgentClient
                {
                    Engine = "Blink",
                    EngineVersion = "123",
                    Name = "Chrome",
                    Type = "Browser",
                    Version = "120.0"
                },
                Device = new UserAgentDevice
                {
                    Brand = "Apple",
                    Model = "iPhone",
                    Type = "Mobile"
                },
                Os = new UserAgentOs
                {
                    Name = "iOS",
                    Platform = "Darwin",
                    Version = "16.5"
                },
                OsFamily = "iOS"
            };

            var dto = model.ToDto();

            Assert.Multiple(() =>
            {
                Assert.That(dto.BrowserFamily, Is.EqualTo(model.BrowserFamily));
                Assert.That(dto.OsFamily, Is.EqualTo(model.OsFamily));

                Assert.That(dto.Client, Is.Not.Null);
            });
            Assert.Multiple(() =>
            {
                Assert.That(dto.Client!.Name, Is.EqualTo(model.Client.Name));

                Assert.That(dto.Device, Is.Not.Null);
            });
            Assert.Multiple(() =>
            {
                Assert.That(dto.Device!.Model, Is.EqualTo(model.Device.Model));

                Assert.That(dto.Os, Is.Not.Null);
            });
            Assert.That(dto.Os!.Version, Is.EqualTo(model.Os.Version));
        }

        [Test]
        public void ToDto_UserAgentApiResponse_AllowsNullNestedObjects()
        {
            var model = new UserAgentApiResponse
            {
                BrowserFamily = "Unknown",
                Client = null,
                Device = null,
                Os = null,
                OsFamily = "Unknown"
            };

            var dto = model.ToDto();

            Assert.Multiple(() =>
            {
                Assert.That(dto.BrowserFamily, Is.EqualTo("Unknown"));
                Assert.That(dto.OsFamily, Is.EqualTo("Unknown"));
                Assert.That(dto.Client, Is.Null);
                Assert.That(dto.Device, Is.Null);
                Assert.That(dto.Os, Is.Null);
            });
        }
    }
}
