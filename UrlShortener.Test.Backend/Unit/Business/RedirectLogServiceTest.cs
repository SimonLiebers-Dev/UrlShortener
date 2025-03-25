using Microsoft.EntityFrameworkCore;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Backend.Models;
using UrlShortener.App.Backend;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.Test.Backend.Unit.Business
{
    [TestFixture]
    public class RedirectLogServiceTests
    {
        private AppDbContext _dbContext;
        private RedirectLogService _service;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext = new AppDbContext(options);
            _service = new RedirectLogService(_dbContext);
        }

        [Test]
        [Description("Test if LogRedirectAsync successfully logs a redirect in the database")]
        public async Task LogRedirectAsync_ShouldLogRedirect()
        {
            // Arrange
            var urlMapping = new UrlMapping { Id = 1, LongUrl = "https://example.com", Path = "exmpl" };
            var userAgentApiResponse = new UserAgentApiResponse
            {
                BrowserFamily = "Chrome",
                Client = new UserAgentClient { Engine = "Blink", Name = "Chrome", Type = "Browser" },
                Device = new UserAgentDevice { Brand = "Google", Model = "Pixel 5", Type = "Mobile" },
                Os = new UserAgentOs { Name = "Android", Version = "11" },
                OsFamily = "Android"
            };
            var ipAddress = "192.168.1.1";
            var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";

            // Act
            await _service.LogRedirectAsync(urlMapping, userAgentApiResponse, ipAddress, userAgent);

            // Assert
            var redirectLog = await _dbContext.RedirectLogs.FirstOrDefaultAsync(r => r.UrlMappingId == urlMapping.Id);

            Assert.That(redirectLog, Is.Not.Null); // Ensures the redirect log is saved in the database
            Assert.Multiple(() =>
            {
                Assert.That(redirectLog.UrlMappingId, Is.EqualTo(urlMapping.Id)); // Verifies the correct mapping is logged
                Assert.That(redirectLog.IpAddress, Is.EqualTo(ipAddress)); // Verifies the IP address is saved
                Assert.That(redirectLog.UserAgent, Is.EqualTo(userAgent)); // Verifies the user agent is saved
                Assert.That(redirectLog.BrowserFamily, Is.EqualTo("Chrome")); // Verifies browser family
                Assert.That(redirectLog.ClientEngine, Is.EqualTo("Blink")); // Verifies the client engine
                Assert.That(redirectLog.DeviceBrand, Is.EqualTo("Google")); // Verifies device brand
                Assert.That(redirectLog.DeviceModel, Is.EqualTo("Pixel 5")); // Verifies device model
                Assert.That(redirectLog.OsName, Is.EqualTo("Android")); // Verifies OS name
                Assert.That(redirectLog.OsVersion, Is.EqualTo("11")); // Verifies OS version
                Assert.That(redirectLog.OsFamily, Is.EqualTo("Android")); // Verifies OS family
            });
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }
    }
}
