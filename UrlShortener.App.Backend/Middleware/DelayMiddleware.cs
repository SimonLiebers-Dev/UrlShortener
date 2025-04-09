namespace UrlShortener.App.Backend.Middleware
{
    /// <summary>
    /// Middleware to introduce a delay for authentication requests.
    /// </summary>
    /// <param name="logger">Logger</param>
    public class DelayMiddleware(ILogger<DelayMiddleware> logger) : IMiddleware
    {
        private readonly TimeSpan _minDelay = TimeSpan.FromMilliseconds(500);
        private readonly TimeSpan _maxJitter = TimeSpan.FromMilliseconds(200);

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Only delay auth requests
            if (context.Request.Path.StartsWithSegments("/api/auth", StringComparison.OrdinalIgnoreCase))
            {
                var delay = _minDelay + TimeSpan.FromMilliseconds(Random.Shared.Next((int)_maxJitter.TotalMilliseconds));
                logger.LogInformation("Delaying auth request by {DELAY}ms", delay.TotalMilliseconds);

                await Task.Delay(delay);
            }

            await next(context);
        }
    }
}
