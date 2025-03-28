using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.App.Backend.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
        }
    }
}
