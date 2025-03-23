using Microsoft.EntityFrameworkCore;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Backend;

namespace UnitTest.Backend.Business
{
    [TestFixture]
    public class MappingsServiceTests
    {
        private AppDbContext _dbContext;
        private MappingsService _service;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext = new AppDbContext(options);
            _service = new MappingsService(_dbContext);
        }

        [Test]
        [Description("Test if CreateMapping successfully creates a new URL mapping")]
        public async Task CreateMapping_ShouldCreateNewMapping()
        {
            // Arrange
            var longUrl = "https://example.com";

            // Act
            var result = await _service.CreateMapping(longUrl, "Test", "user@example.com");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(longUrl, Is.EqualTo(result.LongUrl));
        }

        [Test]
        [Description("Test if GetMappingByPath returns the correct mapping based on the generated path")]
        public async Task GetMappingByPath_ShouldReturnCorrectMapping()
        {
            // Arrange
            var longUrl = "https://example.com";
            var mapping = await _service.CreateMapping(longUrl, "Test", "user@example.com");

            Assert.That(mapping, Is.Not.Null);

            // Act
            var result = await _service.GetMappingByPath(mapping.Path);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(mapping.Path, Is.EqualTo(result.Path));
            Assert.That(longUrl, Is.EqualTo(result.LongUrl));
        }

        [Test]
        [Description("Test if GetMappingsByUser returns mappings for a specific user")]
        public async Task GetMappingsByUser_ShouldReturnUserMappings()
        {
            // Arrange
            var email = "user@example.com";
            var longUrl = "https://example.com";
            await _service.CreateMapping(longUrl, "Test", email);

            // Act
            var result = await _service.GetMappingsByUser(email);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Description("Test if DeleteMapping successfully deletes a URL mapping based on user and ID")]
        public async Task DeleteMapping_ShouldDeleteMapping()
        {
            // Arrange
            var email = "user@example.com";
            var longUrl = "https://example.com";
            var mapping = await _service.CreateMapping(longUrl, "Test", email);

            Assert.That(mapping, Is.Not.Null);

            // Act
            var result = await _service.DeleteMapping(email, mapping.Id);

            // Assert
            Assert.That(result, Is.True);
            var deletedMapping = await _service.GetMappingByPath(mapping.Path);
            Assert.That(deletedMapping, Is.Null);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }
    }
}
