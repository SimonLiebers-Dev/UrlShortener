using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.App.Backend.Controllers
{
    /// <summary>
    /// Simple controller for checking the health status of the API.
    /// Useful for uptime monitoring and infrastructure health checks.
    /// </summary>
    [ApiController]
    [Route("api/health")]
    public class HealthCheckController : ControllerBase
    {
        /// <summary>
        /// Returns a basic health check response.
        /// </summary>
        /// <returns>
        /// <see cref="OkObjectResult"/> containing the current status and a UTC timestamp.
        /// </returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
        }
    }
}
