using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UrlShortener.App.Backend.Business;

namespace UrlShortener.Test.Backend.Unit.Business
{
    [TestFixture]
    public class JwtTokenGeneratorTests
    {
        private Mock<IConfiguration> _mockConfig;
        private JwtTokenGenerator _tokenGenerator;
        private string _secretKey = string.Empty;

        [SetUp]
        public void SetUp()
        {
            _secretKey = Convert.ToBase64String(new byte[32]);

            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c["JwtSettings:SecretKey"]).Returns(_secretKey);
            _mockConfig.Setup(c => c["JwtSettings:Issuer"]).Returns("TestIssuer");
            _mockConfig.Setup(c => c["JwtSettings:Audience"]).Returns("TestAudience");

            _tokenGenerator = new JwtTokenGenerator(_mockConfig.Object);
        }

        [Test]
        [Description("Verifies that the token is generated correctly when a valid email is provided.")]
        public void GenerateToken_ValidEmail_ReturnsToken()
        {
            // Arrange
            var email = "test@example.com";

            // Act
            var token = _tokenGenerator.GenerateToken(email);

            // Assert
            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.Not.Empty);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = "TestIssuer",
                ValidateAudience = true,
                ValidAudience = "TestAudience",
                ValidateLifetime = true
            };
            tokenHandler.ValidateToken(token, validations, out var validatedToken);
            Assert.That(validatedToken, Is.InstanceOf<JwtSecurityToken>());
        }

        [Test]
        [Description("Ensures that an exception is thrown when the secret key is missing in the configuration.")]
        public void GenerateToken_MissingSecretKey_ThrowsException()
        {
            // Arrange
            _mockConfig.Setup(c => c["JwtSettings:SecretKey"]).Returns((string?)null);
            var generator = new JwtTokenGenerator(_mockConfig.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => generator.GenerateToken("test@example.com"));
        }
    }
}
