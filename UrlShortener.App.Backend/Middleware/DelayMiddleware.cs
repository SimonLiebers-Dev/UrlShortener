namespace UrlShortener.App.Backend.Middleware
{
    /// <summary>
    /// Middleware that introduces an artificial delay for authentication-related requests to mitigate brute-force or timing attacks.
    /// </summary>
    /// <remarks>
    /// Applies a base delay of 500ms with up to 200ms of jitter to all requests targeting the <c>/api/auth</c> path.
    /// </remarks>
    public class DelayMiddleware(ILogger<DelayMiddleware> logger) : IMiddleware
    {
        private readonly TimeSpan _minDelay = TimeSpan.FromMilliseconds(500);
        private readonly TimeSpan _maxJitter = TimeSpan.FromMilliseconds(200);

        /// <summary>
        /// Executes the middleware logic, adding a delay to requests under the <c>/api/auth</c> path.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="next">The delegate representing the next middleware in the pipeline.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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
