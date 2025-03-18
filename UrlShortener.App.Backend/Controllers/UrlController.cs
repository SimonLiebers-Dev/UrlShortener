using Microsoft.AspNetCore.Mvc;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Shared.DTO;

namespace UrlShortener.App.Backend.Controllers
{
    [Route("api/url")]
    [ApiController]
    public class UrlController(IMappingsService MappingsService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateMapping([FromBody] ShortenRequestDTO shortenRequest)
        {
            if (string.IsNullOrEmpty(shortenRequest.LongUrl))
            {
                return BadRequest("URL cannot be empty");
            }

            var urlMapping = await MappingsService.CreateMapping(shortenRequest.LongUrl, User.Identity?.Name);

            if (urlMapping == null)
            {
                return BadRequest("URL could not be shortened");
            }

            return Ok(new ShortenResponseDTO { ShortUrl = $"{Request.Scheme}://{Request.Host}/{urlMapping.Path}" });
        }

        [HttpGet("mappings")]
        public async Task<IActionResult> GetMappings()
        {
            var email = User.Identity?.Name;
            if (email == null)
                return NotFound("User not found");

            var userMappings = await MappingsService.GetMappingsByUser(email);

            if (userMappings == null)
                return NotFound("No mappings found");

            return Ok(userMappings.Select(m => new UrlMappingDTO()
            {
                Id = m.Id,
                LongUrl = m.LongUrl,
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{m.Path}",
                CreatedAt = m.CreatedAt,
                User = m.User
            }));
        }
    }
}
