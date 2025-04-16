namespace UrlShortener.App.Blazor.Client.Business
{
    /// <summary>
    /// Provides time zone information based on the user's browser settings.
    /// Inherits from <see cref="TimeProvider"/> and overrides the local time zone with the one provided by the browser.
    /// </summary>
    public class BrowserTimeProvider : TimeProvider
    {
        private TimeZoneInfo? _browserLocalTimeZone;

        /// <summary>
        /// Occurs when the local time zone is changed via <see cref="SetBrowserTimeZone"/>.
        /// </summary>
        public event EventHandler? LocalTimeZoneChanged;

        /// <summary>
        /// Gets the browser-provided local time zone if set; otherwise falls back to the base implementation.
        /// </summary>
        public override TimeZoneInfo LocalTimeZone
            => _browserLocalTimeZone ?? base.LocalTimeZone;

         /// <summary>
        /// Indicates whether the local time zone has been set by the browser.
        /// </summary>
        internal bool IsLocalTimeZoneSet => _browserLocalTimeZone != null;

        /// <summary>
        /// Sets the local time zone based on the provided time zone ID (from the browser).
        /// If the time zone changes, the <see cref="LocalTimeZoneChanged"/> event is raised.
        /// </summary>
        public virtual void SetBrowserTimeZone(string timeZone)
        {
            if (!TimeZoneInfo.TryFindSystemTimeZoneById(timeZone, out var timeZoneInfo))
            {
                timeZoneInfo = null;
            }

            if (timeZoneInfo != LocalTimeZone)
            {
                _browserLocalTimeZone = timeZoneInfo;
                LocalTimeZoneChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
