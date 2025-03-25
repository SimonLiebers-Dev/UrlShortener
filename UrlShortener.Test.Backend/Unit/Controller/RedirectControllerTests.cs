using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Backend.Controllers;
using UrlShortener.App.Backend.Models;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.Test.Backend.Unit.Controller
{
    [TestFixture]
    public class RedirectControllerTests
    {
        private Mock<IMappingsService> _mockMappingsService;
        private Mock<IRedirectLogService> _mockRedirectLogService;
        private Mock<IUserAgentService> _mockUserAgentService;
        private RedirectController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockMappingsService = new Mock<IMappingsService>();
            _mockRedirectLogService = new Mock<IRedirectLogService>();
            _mockUserAgentService = new Mock<IUserAgentService>();

            _controller = new RedirectController(
                _mockMappingsService.Object,
                _mockRedirectLogService.Object,
                _mockUserAgentService.Object
            );
        }

        [Test]
        [Description("Test if RedirectToLongUrl returns NotFound when the path is invalid")]
        public async Task RedirectToLongUrl_InvalidPath_ReturnsNotFound()
        {
            // Arrange
            var path = "invalidpath";

            _mockMappingsService.Setup(m => m.GetMappingByPath(It.IsAny<string>()))
                .ReturnsAsync((UrlMapping?)null);

            // Act
            var result = await _controller.RedirectToLongUrl(path);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        [Description("Test if RedirectToLongUrl logs the redirect when valid path is provided")]
        public async Task RedirectToLongUrl_ValidPath_LogsRedirect()
        {
            // Arrange
            var path = "shorturl123";
            var urlMapping = new UrlMapping
            {
                LongUrl = "https://example.com"
            };
            var userAgent = "Mozilla/5.0";
            var userAgentData = new UserAgentApiResponse(); // Simulated response from the UserAgentService
            var ipAddress = "192.168.1.1";

            _mockMappingsService.Setup(m => m.GetMappingByPath(It.IsAny<string>()))
                .ReturnsAsync(urlMapping);
            _mockUserAgentService.Setup(u => u.GetUserAgentAsync(It.IsAny<string>()))
                .ReturnsAsync(userAgentData);

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Connection.RemoteIpAddress = IPAddress.Parse(ipAddress);
            _controller.ControllerContext.HttpContext.Request.Headers.UserAgent = userAgent;

            // Act
            var result = await _controller.RedirectToLongUrl(path);

            // Assert 
            _mockRedirectLogService.Verify(r => r.LogRedirectAsync(urlMapping, userAgentData, ipAddress, userAgent), Times.Once);

            Assert.That(result, Is.InstanceOf<RedirectResult>());

            var response = result as RedirectResult;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Url, Is.EqualTo(urlMapping.LongUrl));
        }
    }
}
