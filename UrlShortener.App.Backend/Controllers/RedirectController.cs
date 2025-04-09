using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Backend.Models;

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
            // Get url mapping from db
            var urlMapping = await MappingsService.GetMappingByPath(path);

            // If the mapping is not found, return a 404 Not Found response
            if (urlMapping == null)
                return NotFound();

            // Get the IP address of the user from the request
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Get the user agent string from the request headers
            var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

            // Only fetch user agent data if the user agent string is not null or empty
            UserAgentApiResponse? userAgentData = null;
            if (!string.IsNullOrEmpty(userAgent))
                userAgentData = await UserAgentService.GetUserAgentAsync(userAgent);

            // Log the redirect in db
            await RedirectLogService.LogRedirectAsync(urlMapping, userAgentData, ipAddress, userAgent);

            // Redirect to the long URL
            return Redirect(urlMapping.LongUrl);
        }
    }
}
