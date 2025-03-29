using UrlShortener.App.Blazor.Client.Business;

namespace UrlShortener.Test.Frontend.Unit.Business
{
    [TestFixture]
    public class BrowserTimeProviderTests
    {
        private BrowserTimeProvider _timeProvider;

        [SetUp]
        public void SetUp()
        {
            _timeProvider = new BrowserTimeProvider();
        }

        [Test]
        public void LocalTimeZone_Default_ReturnsBaseLocalTimeZone()
        {
            // Act
            var timeZone = _timeProvider.LocalTimeZone;

            // Assert
            Assert.That(timeZone, Is.EqualTo(TimeZoneInfo.Local));
        }

        [Test]
        public void SetBrowserTimeZone_ValidTimeZone_UpdatesLocalTimeZone()
        {
            // Arrange
            var expectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

            // Act
            _timeProvider.SetBrowserTimeZone("Pacific Standard Time");

            // Assert
            Assert.That(_timeProvider.LocalTimeZone, Is.EqualTo(expectedTimeZone));
        }

        [Test]
        public void SetBrowserTimeZone_InvalidTimeZone_DoesNotChangeLocalTimeZone()
        {
            // Arrange
            var originalTimeZone = _timeProvider.LocalTimeZone;

            // Act
            _timeProvider.SetBrowserTimeZone("Invalid TimeZone");

            // Assert
            Assert.That(_timeProvider.LocalTimeZone, Is.EqualTo(originalTimeZone));
        }

        [Test]
        public void SetBrowserTimeZone_NewTimeZone_RaisesLocalTimeZoneChangedEvent()
        {
            // Arrange
            bool eventRaised = false;
            _timeProvider.LocalTimeZoneChanged += (s, e) => eventRaised = true;

            // Act
            _timeProvider.SetBrowserTimeZone("Pacific Standard Time");

            // Assert
            Assert.That(eventRaised, Is.True);
        }

        [Test]
        public void SetBrowserTimeZone_SameTimeZone_DoesNotRaiseEvent()
        {
            // Arrange
            _timeProvider.SetBrowserTimeZone("Pacific Standard Time");
            bool eventRaised = false;
            _timeProvider.LocalTimeZoneChanged += (s, e) => eventRaised = true;

            // Act
            _timeProvider.SetBrowserTimeZone("Pacific Standard Time");

            // Assert
            Assert.That(eventRaised, Is.False);
        }
    }

}
