using Microsoft.AspNetCore.Http;
using Moq;
using UrlShortener.App.Shared.Extensions;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.Test.Shared.Unit
{
    [TestFixture]
    public class DtoExtensionTests
    {
        [Test]
        public void ToDto_UrlMapping_ReturnsCorrectDto()
        {
            var mapping = new UrlMapping
            {
                Id = 1,
                LongUrl = "https://example.com",
                Name = "Example",
                Path = "abc123",
                CreatedAt = new DateTime(2024, 1, 1),
                User = "testUser",
                RedirectLogs =
                [
                    new RedirectLog { Id = 100, IpAddress = "127.0.0.1", UserAgent = "Chrome", AccessedAt = DateTime.UtcNow }
                ]
            };

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(r => r.Scheme).Returns("https");
            mockRequest.Setup(r => r.Host).Returns(new HostString("localhost", 5001));

            var dto = mapping.ToDto(mockRequest.Object);

            Assert.Multiple(() =>
            {
                Assert.That(dto.Id, Is.EqualTo(mapping.Id));
                Assert.That(dto.LongUrl, Is.EqualTo(mapping.LongUrl));
                Assert.That(dto.Name, Is.EqualTo(mapping.Name));
                Assert.That(dto.ShortUrl, Is.EqualTo("https://localhost:5001/abc123"));
                Assert.That(dto.CreatedAt, Is.EqualTo(mapping.CreatedAt));
                Assert.That(dto.User, Is.EqualTo(mapping.User));
                Assert.That(dto.RedirectLogs, Has.Count.EqualTo(1));
            });
        }

        [Test]
        public void ToDto_RedirectLog_ReturnsCorrectDto()
        {
            var log = new RedirectLog
            {
                Id = 1,
                IpAddress = "192.168.1.1",
                UserAgent = "Mozilla",
                AccessedAt = new DateTime(2025, 6, 1),
                BrowserFamily = "Chrome",
                ClientEngine = "Blink",
                ClientName = "Chrome",
                ClientType = "Browser",
                DeviceBrand = "BrandX",
                DeviceModel = "ModelY",
                DeviceType = "Mobile",
                OsName = "Android",
                OsVersion = "12",
                OsFamily = "Android"
            };

            var dto = log.ToDto();

            Assert.Multiple(() =>
            {
                Assert.That(dto.Id, Is.EqualTo(log.Id));
                Assert.That(dto.IpAddress, Is.EqualTo(log.IpAddress));
                Assert.That(dto.UserAgent, Is.EqualTo(log.UserAgent));
                Assert.That(dto.AccessedAt, Is.EqualTo(log.AccessedAt));
                Assert.That(dto.BrowserFamily, Is.EqualTo(log.BrowserFamily));
                Assert.That(dto.ClientEngine, Is.EqualTo(log.ClientEngine));
                Assert.That(dto.ClientName, Is.EqualTo(log.ClientName));
                Assert.That(dto.ClientType, Is.EqualTo(log.ClientType));
                Assert.That(dto.DeviceBrand, Is.EqualTo(log.DeviceBrand));
                Assert.That(dto.DeviceModel, Is.EqualTo(log.DeviceModel));
                Assert.That(dto.DeviceType, Is.EqualTo(log.DeviceType));
                Assert.That(dto.OsName, Is.EqualTo(log.OsName));
                Assert.That(dto.OsVersion, Is.EqualTo(log.OsVersion));
                Assert.That(dto.OsFamily, Is.EqualTo(log.OsFamily));
            });
        }

        [Test]
        public void GetStats_ReturnsAggregatedStats()
        {
            var logs = new List<RedirectLog>
            {
                new RedirectLog { AccessedAt = new DateTime(2025, 6, 1), DeviceType = "Mobile" },
                new RedirectLog { AccessedAt = new DateTime(2025, 6, 1), DeviceType = "Mobile" },
                new RedirectLog { AccessedAt = new DateTime(2025, 6, 2), DeviceType = "Desktop" }
            };

            var mappings = new List<UrlMapping>
            {
                new() { Id = 1, Name = "Link1", RedirectLogs = logs }
            };

            var stats = mappings.GetStats();

            Assert.Multiple(() =>
            {
                Assert.That(stats.Clicks, Is.EqualTo(3));
                Assert.That(stats.Mappings, Is.EqualTo(1));
                Assert.That(stats.DeviceTypeStats.Count, Is.EqualTo(2));
            });
            Assert.That(stats.DeviceTypeStats.FirstOrDefault(d => d.DeviceType == "Mobile")?.Clicks, Is.EqualTo(2));
            Assert.That(stats.TimeSeriesStats.Count, Is.EqualTo(1));
            Assert.That(stats.TimeSeriesStats.First().ClicksPerDay.Sum(c => c.Clicks), Is.EqualTo(3));
        }
    }
}
