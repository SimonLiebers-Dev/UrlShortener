using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Net;
using UrlShortener.App.Backend;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Backend.Models;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.Test.Backend.Integration.Controllers
{
    [TestFixture]
    public class RedirectControllerTests : WebApplicationFactory<Program>
    {
        private HttpClient _httpClient;
        private Mock<IUserAgentService> _userAgentService;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddEntityFrameworkInMemoryDatabase();

                var provider = services
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                services.RemoveAll<IUserAgentService>();

                _userAgentService = new Mock<IUserAgentService>();
                services.AddScoped(_ => _userAgentService.Object);

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();

                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();

                db.Database.EnsureCreated();

                var testMapping = new UrlMapping()
                {
                    LongUrl = "https://test.com/",
                    Name = "Test",
                    Path = "ValidPath",
                    CreatedAt = DateTime.UtcNow,
                    User = "test@gmail.com",
                    RedirectLogs = []
                };
                db.UrlMappings.Add(testMapping);

                db.SaveChanges();
            });
        }

        [SetUp]
        public void Setup()
        {
            _userAgentService = new Mock<IUserAgentService>();
            
            _httpClient = CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Test]
        public async Task RedirectToLongUrl_InvalidPath_ReturnsNotFound()
        {
            // Act
            var response = await _httpClient.GetAsync("InvalidPath");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task RedirectToLongUrl_ValidPath_ReturnsRedirect()
        {
            // Arrange 
            _userAgentService.Setup(s => s.GetUserAgentAsync(It.IsAny<string>())).ReturnsAsync(new UserAgentApiResponse());

            // Act
            var response = await _httpClient.GetAsync("ValidPath");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));
            _userAgentService.Verify(s => s.GetUserAgentAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
