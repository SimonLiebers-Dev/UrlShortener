namespace UrlShortener.App.Blazor.Client.Extensions
{
    internal static class TimeProviderExtensions
    {
        public static DateTime ToLocalDateTime(this TimeProvider timeProvider, DateTime dateTime)
        {
            return dateTime.Kind switch
            {
                DateTimeKind.Local => dateTime,
                _ => DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeProvider.LocalTimeZone), DateTimeKind.Local),
            };
        }

        public static DateTime ToLocalDateTime(this TimeProvider timeProvider, DateTimeOffset dateTime)
        {
            var local = TimeZoneInfo.ConvertTimeFromUtc(dateTime.UtcDateTime, timeProvider.LocalTimeZone);
            local = DateTime.SpecifyKind(local, DateTimeKind.Local);
            return local;
        }
    }
}
