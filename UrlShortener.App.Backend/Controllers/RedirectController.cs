using Microsoft.AspNetCore.Mvc;
using UrlShortener.App.Backend.Business;

namespace UrlShortener.App.Backend.Controllers
{
    [Route("/")]
    [ApiController]
    public class RedirectController(IMappingsService MappingsService, IRedirectLogService RedirectLogService) : ControllerBase
    {
        [HttpGet("{path}")]
        public async Task<IActionResult> RedirectToLongUrl(string path)
        {
            var urlMapping = await MappingsService.GetMappingByPath(path);

            if (urlMapping == null)
                return NotFound();

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

            // TODO: Fetch location
            await RedirectLogService.LogRedirectAsync(urlMapping, ipAddress, userAgent, null, null);

            return Redirect(urlMapping.LongUrl);
        }
    }
}
