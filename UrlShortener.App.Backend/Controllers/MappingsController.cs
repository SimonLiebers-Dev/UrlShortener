using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Shared.Dto;
using UrlShortener.App.Shared.Extensions;

namespace UrlShortener.App.Backend.Controllers
{
    [ApiController]
    [Route("api/mappings")]
    [Authorize]
    internal class MappingsController(IMappingsService MappingsService) : ControllerBase
    {
        private const string UserNotFoundMessage = "User not found";

        [HttpPost("create")]
        public async Task<IActionResult> CreateMapping([FromBody] CreateMappingRequestDto createMappingRequest)
        {
            var email = User.Identity?.Name;
            if (email == null)
                return NotFound(UserNotFoundMessage);

            if (string.IsNullOrEmpty(createMappingRequest.LongUrl))
                return BadRequest("URL cannot be empty");

            if (string.IsNullOrEmpty(createMappingRequest.Name))
                return BadRequest("Name cannot be empty");

            var urlMapping = await MappingsService.CreateMapping(createMappingRequest.LongUrl, createMappingRequest.Name, email);

            if (urlMapping == null)
                return BadRequest("URL could not be shortened");

            return Ok(new CreateMappingResponseDto
            {
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{urlMapping.Path}"
            });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetMappings()
        {
            var email = User.Identity?.Name;
            if (email == null)
                return NotFound(UserNotFoundMessage);

            var userMappings = await MappingsService.GetMappingsByUser(email);

            if (userMappings == null)
                return NotFound("No mappings found");

            return Ok(userMappings.Select(m => m.ToDto(Request)));
        }

        [HttpDelete("{mappingId}")]
        public async Task<IActionResult> DeleteMapping(int mappingId)
        {
            var email = User.Identity?.Name;
            if (email == null)
                return NotFound(UserNotFoundMessage);

            bool success = await MappingsService.DeleteMapping(email, mappingId);

            if (!success)
                return BadRequest("Something went wrong");

            return Ok("Successfully deleted");
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var email = User.Identity?.Name;
            if (email == null)
                return NotFound(UserNotFoundMessage);

            var userMappings = await MappingsService.GetMappingsByUser(email);

            if (userMappings == null)
                return Ok(new UserStatsDto());

            return Ok(new UserStatsDto()
            {
                Clicks = userMappings.SelectMany(s => s.RedirectLogs).Count(),
                Mappings = userMappings.Count
            });
        }
    }
}
