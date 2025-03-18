using Microsoft.AspNetCore.Mvc;
using UrlShortener.App.Backend.Business;

namespace UrlShortener.App.Backend.Controllers
{
    [Route("/")]
    [ApiController]
    public class RedirectController(IMappingsService MappingsService) : ControllerBase
    {
        [HttpGet("{path}")]
        public async Task<IActionResult> RedirectToLongUrl(string path)
        {
            var urlMapping = await MappingsService.GetMappingByPath(path);

            if (urlMapping == null)
            {
                return NotFound();
            }
            return Redirect(urlMapping.LongUrl);
        }
    }
}
