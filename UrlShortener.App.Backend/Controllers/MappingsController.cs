using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.App.Backend.Business;
using UrlShortener.App.Shared.Dto;
using UrlShortener.App.Shared.Extensions;

namespace UrlShortener.App.Backend.Controllers
{
    /// <summary>
    /// Controller responsible for managing URL mappings for authenticated users.
    /// Allows users to create, retrieve, delete, and view statistics for their shortened URLs.
    /// </summary>
    [ApiController]
    [Route("api/mappings")]
    [Authorize]
    public class MappingsController(IMappingsService MappingsService) : ControllerBase
    {
        private const string UserNotFoundMessage = "User not found";

        /// <summary>
        /// Creates a new URL mapping (shortens a URL).
        /// </summary>
        /// <param name="createMappingRequest">Request containing the long URL and a name for the mapping.</param>
        /// <returns>
        /// <see cref="OkObjectResult"/> with a <see cref="CreateMappingResponseDto"/> containing the shortened URL,
        /// or <see cref="BadRequestObjectResult"/> if the input is invalid or creation fails.
        /// </returns>
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
                ShortUrl = $"{Request.Scheme}://localhost:{Request.Host.Port}/{urlMapping.Path}"
            });
        }

        /// <summary>
        /// Retrieves all URL mappings for the authenticated user.
        /// </summary>
        /// <returns>
        /// <see cref="OkObjectResult"/> with a list of mappings, or <see cref="NotFoundObjectResult"/> if no mappings exist.
        /// </returns>
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

        /// <summary>
        /// Deletes a specific URL mapping by ID.
        /// </summary>
        /// <param name="mappingId">The ID of the mapping to delete.</param>
        /// <returns>
        /// <see cref="OkObjectResult"/> if the deletion is successful,
        /// or <see cref="BadRequestObjectResult"/> if deletion fails.
        /// </returns>
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

        /// <summary>
        /// Retrieves statistics for the authenticated user, such as total number of mappings and total clicks.
        /// </summary>
        /// <returns>
        /// <see cref="OkObjectResult"/> containing a <see cref="UserStatsDto"/> with the user's statistics.
        /// </returns>
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
