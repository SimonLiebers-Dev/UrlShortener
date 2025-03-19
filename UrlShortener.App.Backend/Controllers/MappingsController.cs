using Microsoft.AspNetCore.Mvc;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Shared.DTO;
using UrlShortener.App.Shared.Extensions;

namespace UrlShortener.App.Backend.Controllers
{
    [Route("api/mappings")]
    [ApiController]
    public class MappingsController(IMappingsService MappingsService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateMapping([FromBody] CreateMappingRequestDTO createMappingRequest)
        {
            if (string.IsNullOrEmpty(createMappingRequest.LongUrl))
                return BadRequest("URL cannot be empty");

            var urlMapping = await MappingsService.CreateMapping(createMappingRequest.LongUrl, createMappingRequest.Name, User.Identity?.Name);

            if (urlMapping == null)
                return BadRequest("URL could not be shortened");

            return Ok(new CreateMappingResponseDTO
            {
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{urlMapping.Path}"
            });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetMappings()
        {
            var email = User.Identity?.Name;
            if (email == null)
                return NotFound("User not found");

            var userMappings = await MappingsService.GetMappingsByUser(email);

            if (userMappings == null)
                return NotFound("No mappings found");

            return Ok(userMappings.Select(m => m.ToDTO(Request)));
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var email = User.Identity?.Name;
            if (email == null)
                return NotFound("User not found");

            var userMappings = await MappingsService.GetMappingsByUser(email);

            if (userMappings == null)
                return Ok(new UserStatsDTO());

            return Ok(new UserStatsDTO()
            {
                Clicks = userMappings.SelectMany(s => s.RedirectLogs).Count(),
                Mappings = userMappings.Count
            });
        }
    }
}
