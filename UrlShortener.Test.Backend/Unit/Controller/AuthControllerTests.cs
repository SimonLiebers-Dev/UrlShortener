using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Backend.Controllers;
using UrlShortener.App.Backend.Utils;
using UrlShortener.App.Backend;
using UrlShortener.App.Shared.Dto;
using UrlShortener.App.Shared.Models;
using Microsoft.AspNetCore.Http;

namespace UrlShortener.Test.Backend.Unit.Controller
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
        private AppDbContext _dbContext;
        private AuthController _controller;

        [OneTimeSetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext = new AppDbContext(options);

            _dbContext.Users.Add(new User
            {
                Email = "testuser@example.com",
                PasswordHash = PasswordUtils.HashPassword("TestPassword", "salt"),
                Salt = "salt"
            });
            _dbContext.SaveChanges();

            _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            _mockJwtTokenGenerator.Setup(m => m.GenerateToken(It.IsAny<string>())).Returns("mockedJwtToken");

            _controller = new AuthController(_mockJwtTokenGenerator.Object, _dbContext);
        }

        [Test]
        [Description("Test if Login returns valid token when valid email and password are provided")]
        public void Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "testuser@example.com",
                Password = "TestPassword"
            };

            // Act
            var result = _controller.Login(loginRequest) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
                Assert.That(result.Value, Is.InstanceOf<LoginResponseDto>());
            });

            var response = result.Value as LoginResponseDto;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Token, Is.EqualTo("mockedJwtToken"));
        }

        [Test]
        [Description("Test if Login returns Unauthorized for invalid email")]
        public void Login_InvalidEmail_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "nonexistentuser@example.com",
                Password = "TestPassword"
            };

            // Act
            var result = _controller.Login(loginRequest) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status401Unauthorized));
        }

        [Test]
        [Description("Test if Register returns Ok with success when valid data is provided")]
        public async Task Register_ValidData_ReturnsOk()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "newuser@example.com",
                Password = "NewUserPassword"
            };

            // Act
            var result = await _controller.Register(registerRequest) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
                Assert.That(result.Value, Is.InstanceOf<RegisterResponseDto>());
            });

            var response = result.Value as RegisterResponseDto;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Success, Is.True);
        }

        [Test]
        [Description("Test if Register returns Ok with error for missing email or password")]
        public async Task Register_MissingEmail_ReturnsError()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "",
                Password = "NewUserPassword"
            };

            // Act
            var result = await _controller.Register(registerRequest) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
                Assert.That(result.Value, Is.InstanceOf<RegisterResponseDto>());
            });

            var response = result.Value as RegisterResponseDto;

            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.Success, Is.False);
                Assert.That(response.ErrorType, Is.EqualTo(RegisterErrorType.MissingEmailOrPassword));
            });
        }

        [Test]
        [Description("Test if Register returns error for existing email")]
        public async Task Register_EmailAlreadyExists_ReturnsError()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "testuser@example.com",
                Password = "NewUserPassword"
            };

            // Act
            var result = await _controller.Register(registerRequest) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
                Assert.That(result.Value, Is.InstanceOf<RegisterResponseDto>());
            });

            var response = result.Value as RegisterResponseDto;

            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.Success, Is.False);
                Assert.That(response.ErrorType, Is.EqualTo(RegisterErrorType.EmailAlreadyExists));
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }
    }
}
