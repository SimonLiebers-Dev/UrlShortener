using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.App.Backend.Business;

namespace UrlShortener.App.Backend.Controllers
{
    [ApiController]
    [Route("/")]
    [AllowAnonymous]
    public class RedirectController(IMappingsService MappingsService, IRedirectLogService RedirectLogService, IUserAgentService UserAgentService) : ControllerBase
    {
        /// <summary>
        /// Redirects a short URL to its original long URL.
        /// </summary>
        /// <param name="path">The short URL path.</param>
        /// <returns>A redirect response to the original long URL.</returns>
        [HttpGet("{path}")]
        public async Task<IActionResult> RedirectToLongUrl(string path)
        {
            var urlMapping = await MappingsService.GetMappingByPath(path);

            if (urlMapping == null)
                return NotFound();

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var userAgent = HttpContext.Request.Headers.UserAgent.ToString();
            var userAgentData = await UserAgentService.GetUserAgentAsync(userAgent);

            await RedirectLogService.LogRedirectAsync(urlMapping, userAgentData, ipAddress, userAgent);

            return Redirect(urlMapping.LongUrl);
        }
    }
}
