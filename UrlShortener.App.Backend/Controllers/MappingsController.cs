using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
    [EnableRateLimiting("fixed")]
    [Authorize]
    public class MappingsController(ILogger<MappingsController> Logger, IMappingsService MappingsService) : ControllerBase
    {
        private const string UserNotFoundMessage = "User not found";
        private HtmlSanitizer _sanitizer = new HtmlSanitizer();

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
            // Get the email of the authenticated user
            var email = User.Identity?.Name;

            Logger.LogInformation("CreateMapping(email={Email}, longUrl={LongUrl})", email, createMappingRequest.LongUrl);

            // Check if email exists
            if (email == null)
            {
                return NotFound(UserNotFoundMessage);
            }

            // Validate the request data
            if (string.IsNullOrEmpty(createMappingRequest.LongUrl))
            {
                return BadRequest("URL cannot be empty");
            }

            // Check if the name is valid
            if (string.IsNullOrEmpty(createMappingRequest.Name))
            {
                return BadRequest("Name cannot be empty");
            }

            // Sanitize name to prevent xss attacks
            string cleanName = _sanitizer.Sanitize(createMappingRequest.Name);
            if (string.IsNullOrEmpty(cleanName))
            {
                return BadRequest("Name cannot be empty");
            }

            // Create the URL mapping
            var urlMapping = await MappingsService.CreateMapping(createMappingRequest.LongUrl, cleanName, email);

            // If the mapping could not be created, return a bad request
            if (urlMapping == null)
            {
                return BadRequest("URL could not be shortened");
            }

            Logger.LogInformation("CreateMapping(email={Email}, longUrl={LongUrl}) - Mapping successfully created", email, createMappingRequest.LongUrl);

            // Return the response with the shortened URL
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
            // Get the email of the authenticated user
            var email = User.Identity?.Name;

            Logger.LogInformation("GetMappings(email={Email})", email);

            // Check if email exists
            if (email == null)
            {
                return NotFound(UserNotFoundMessage);
            }

            // Retrieve all mappings for the user
            var userMappings = await MappingsService.GetMappingsByUser(email);

            // If no mappings are found, return a not found response
            if (userMappings == null)
            {
                return NotFound("No mappings found");
            }

            // Convert the mappings to DTOs and return them
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
            // Get the email of the authenticated user
            var email = User.Identity?.Name;

            Logger.LogInformation("DeleteMapping(email={Email})", email);

            // Check if email exists
            if (email == null)
            {
                return NotFound(UserNotFoundMessage);
            }

            // Check if mappingId is valid
            bool success = await MappingsService.DeleteMapping(email, mappingId);

            // If deletion was not successful, return a bad request
            if (!success)
            {
                return BadRequest("Something went wrong");
            }

            Logger.LogInformation("DeleteMapping(email={Email}, mappingId={MappingId}) - Successfully deleted", email, mappingId);

            // Return success message
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
            // Get the email of the authenticated user
            var email = User.Identity?.Name;

            Logger.LogInformation("GetStats(email={Email})", email);

            // Check if email exists
            if (email == null)
            {
                return NotFound(UserNotFoundMessage);
            }

            // Retrieve mappings for the user
            var userMappings = await MappingsService.GetMappingsByUser(email);

            // If no mappings are found, return an empty object
            if (userMappings == null)
            {
                return Ok(new UserStatsDto());
            }

            // Convert the mappings to a DTO and return the statistics
            return Ok(userMappings.GetStats());
        }
    }
}
