using UrlShortener.App.Backend.Utils;
using UrlShortener.App.Shared.Models;

namespace UrlShortener.Test.Backend.Unit.Utils
{
    [TestFixture]
    public class DataSeederTests
    {
        [Test]
        public void GetDemoRedirectLogs_GeneratesLogsWithinTimeRangeAndMultiplePerTimestamp()
        {
            // Arrange
            var user = new User { Email = "test@example.com", Id = 1 };
            var mapping = new UrlMapping
            {
                Id = 1,
                Path = "abc123",
                CreatedAt = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                User = user.Email,
                RedirectLogs = []
            };

            var from = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc);
            var to = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc);
            var numberOfEntries = 10;

            // Act
            var logs = InvokePrivateRedirectLogGenerator(user, mapping, from, to, numberOfEntries);

            // Assert
            Assert.That(logs, Is.Not.Empty, "The generated log list should not be empty.");
            Assert.That(logs, Has.Count.GreaterThanOrEqualTo(numberOfEntries), "Total logs must be >= numberOfEntries due to multiple clicks per timestamp.");

            var distinctTimestamps = logs.Select(l => l.AccessedAt).Distinct().ToList();
            Assert.Multiple(() =>
            {
                Assert.That(distinctTimestamps, Has.Count.EqualTo(numberOfEntries), "There should be exactly numberOfEntries unique timestamps.");

                Assert.That(logs.All(log => log.AccessedAt >= from && log.AccessedAt <= to), Is.True,
                    "All AccessedAt values must lie within the specified range.");
            });

            // Check that there are duplicate timestamps (i.e., multiple clicks per point)
            var grouped = logs.GroupBy(l => l.AccessedAt).Where(g => g.Count() > 1);
            Assert.That(grouped.Any(), Is.True, "There should be at least one timestamp with multiple clicks.");
        }

        // Helper to invoke private method via reflection
        private static List<RedirectLog> InvokePrivateRedirectLogGenerator(User user, UrlMapping mapping, DateTime from, DateTime to, int numberOfEntries)
        {
            var method = typeof(DataSeeder).GetMethod("GetDemoRedirectLogs", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            return method?.Invoke(null, [user, mapping, from, to, numberOfEntries]) as List<RedirectLog>
                ?? throw new InvalidOperationException("Unable to invoke GetDemoRedirectLogs.");
        }
    }
}
