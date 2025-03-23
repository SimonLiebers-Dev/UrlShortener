using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework.Constraints;
using System.Linq;
using System.Security.Claims;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Backend.Controllers;
using UrlShortener.App.Shared.Dto;
using UrlShortener.App.Shared.Models;

namespace UnitTest.Backend.Controller
{
    [TestFixture]
    public class MappingsControllerTests
    {
        private Mock<IMappingsService> _mockMappingsService;
        private MappingsController _controller;
        private ClaimsPrincipal _user;

        [SetUp]
        public void SetUp()
        {
            _mockMappingsService = new Mock<IMappingsService>();

            _user = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, "testuser@example.com")
            ], "mock"));

            _controller = new MappingsController(_mockMappingsService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = _user
                    }
                }
            };
        }

        [Test]
        [Description("Test if CreateMapping returns Ok with shortened URL when valid data is provided")]
        public async Task CreateMapping_ValidData_ReturnsOkWithShortUrl()
        {
            // Arrange
            var createMappingRequest = new CreateMappingRequestDto
            {
                LongUrl = "https://example.com",
                Name = "Example"
            };

            var urlMapping = new UrlMapping
            {
                Path = "shorturl123"
            };

            _mockMappingsService.Setup(m => m.CreateMapping(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(urlMapping);

            // Act
            var result = await _controller.CreateMapping(createMappingRequest) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
                Assert.That(result.Value, Is.InstanceOf<CreateMappingResponseDto>());
            });

            var response = result.Value as CreateMappingResponseDto;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.ShortUrl, Is.Not.Empty);
        }

        [Test]
        [Description("Test if CreateMapping returns BadRequest when URL is missing")]
        public async Task CreateMapping_MissingUrl_ReturnsBadRequest()
        {
            // Arrange
            var createMappingRequest = new CreateMappingRequestDto
            {
                LongUrl = "",
                Name = "Example"
            };

            // Act
            var result = await _controller.CreateMapping(createMappingRequest) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }

        [Test]
        [Description("Test if CreateMapping returns BadRequest when name is missing")]
        public async Task CreateMapping_MissingName_ReturnsBadRequest()
        {
            // Arrange
            var createMappingRequest = new CreateMappingRequestDto
            {
                LongUrl = "https://example.com",
                Name = ""
            };

            // Act
            var result = await _controller.CreateMapping(createMappingRequest) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }

        [Test]
        [Description("Test if GetMappings returns Ok with mappings for the authenticated user")]
        public async Task GetMappings_ValidUser_ReturnsOkWithMappings()
        {
            // Arrange
            var userMappings = new List<UrlMapping>
            {
                new() { Path = "shorturl123", LongUrl = "https://example.com" }
            };

            _mockMappingsService.Setup(m => m.GetMappingsByUser(It.IsAny<string>()))
                .ReturnsAsync(userMappings);

            // Act
            var result = await _controller.GetMappings() as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
                Assert.That(result.Value, Is.InstanceOf<IEnumerable<UrlMappingDto>>());
            });

            var response = result.Value as IEnumerable<UrlMappingDto>;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.ToList(), Has.Count.EqualTo(1));
        }

        [Test]
        [Description("Test if GetMappings returns NotFound when no mappings exist for the user")]
        public async Task GetMappings_NoMappingsFound_ReturnsNotFound()
        {
            // Arrange
            _mockMappingsService.Setup(m => m.GetMappingsByUser(It.IsAny<string>()))
                .ReturnsAsync((List<UrlMapping>?)null);

            // Act
            var result = await _controller.GetMappings() as NotFoundObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }

        // Test for DeleteMapping
        [Test]
        [Description("Test if DeleteMapping returns Ok when mapping is deleted successfully")]
        public async Task DeleteMapping_ValidMappingId_ReturnsOk()
        {
            // Arrange
            var mappingId = 1;
            _mockMappingsService.Setup(m => m.DeleteMapping(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteMapping(mappingId) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        // Test for DeleteMapping when deletion fails
        [Test]
        [Description("Test if DeleteMapping returns BadRequest when deletion fails")]
        public async Task DeleteMapping_FailedDeletion_ReturnsBadRequest()
        {
            // Arrange
            var mappingId = 1;
            _mockMappingsService.Setup(m => m.DeleteMapping(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteMapping(mappingId) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }

        // Test for GetStats
        [Test]
        [Description("Test if GetStats returns Ok with stats for the authenticated user")]
        public async Task GetStats_ValidUser_ReturnsOkWithStats()
        {
            // Arrange
            var userMappings = new List<UrlMapping>
            {
                new() { RedirectLogs = [new()] },
                new() { RedirectLogs = [new(), new()] }
            };

            _mockMappingsService.Setup(m => m.GetMappingsByUser(It.IsAny<string>()))
                .ReturnsAsync(userMappings);

            // Act
            var result = await _controller.GetStats() as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
                Assert.That(result.Value, Is.InstanceOf<UserStatsDto>());
            });

            var response = result.Value as UserStatsDto;

            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.Clicks, Is.EqualTo(3));
                Assert.That(response.Mappings, Is.EqualTo(2));
            });
        }
    }
}
