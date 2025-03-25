using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.App.Frontend.Business;

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
        public void LocalTimeZone_Default_ReturnsSystemLocalTimeZone()
        {
            // Act
            var localTimeZone = _timeProvider.LocalTimeZone;

            // Assert
            Assert.That(localTimeZone, Is.EqualTo(TimeZoneInfo.Local));
        }

        [Test]
        public void SetBrowserTimeZone_ValidTimeZone_UpdatesLocalTimeZone()
        {
            // Arrange
            var expectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

            // Act
            _timeProvider.SetBrowserTimeZone("Pacific Standard Time");

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(_timeProvider.LocalTimeZone, Is.EqualTo(expectedTimeZone));
                Assert.That(_timeProvider.IsLocalTimeZoneSet, Is.True);
            });
        }

        [Test]
        public void SetBrowserTimeZone_InvalidTimeZone_DoesNotUpdateLocalTimeZone()
        {
            // Arrange
            var originalTimeZone = _timeProvider.LocalTimeZone;

            // Act
            _timeProvider.SetBrowserTimeZone("Invalid Time Zone");

            // Assert
            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(_timeProvider.LocalTimeZone, Is.EqualTo(originalTimeZone));
                Assert.That(_timeProvider.IsLocalTimeZoneSet, Is.False);
            });
        }

        [Test]
        public void SetBrowserTimeZone_TimeZoneChanged_EventIsTriggered()
        {
            // Arrange
            bool eventTriggered = false;
            _timeProvider.LocalTimeZoneChanged += (sender, args) => eventTriggered = true;

            // Act
            _timeProvider.SetBrowserTimeZone("Pacific Standard Time");

            // Assert
            Assert.That(eventTriggered, Is.True);
        }

        [Test]
        public void SetBrowserTimeZone_SameTimeZone_NoEventTriggered()
        {
            // Arrange
            _timeProvider.SetBrowserTimeZone("Pacific Standard Time");
            bool eventTriggered = false;
            _timeProvider.LocalTimeZoneChanged += (sender, args) => eventTriggered = true;

            // Act
            _timeProvider.SetBrowserTimeZone("Pacific Standard Time");

            // Assert
            Assert.That(eventTriggered, Is.False);
        }
    }
}
