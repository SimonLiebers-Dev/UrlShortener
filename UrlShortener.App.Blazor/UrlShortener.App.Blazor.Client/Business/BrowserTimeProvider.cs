namespace UrlShortener.App.Blazor.Client.Business
{
    public class BrowserTimeProvider : TimeProvider
    {
        private TimeZoneInfo? _browserLocalTimeZone;

        public event EventHandler? LocalTimeZoneChanged;

        public override TimeZoneInfo LocalTimeZone
            => _browserLocalTimeZone ?? base.LocalTimeZone;

        internal bool IsLocalTimeZoneSet => _browserLocalTimeZone != null;

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
