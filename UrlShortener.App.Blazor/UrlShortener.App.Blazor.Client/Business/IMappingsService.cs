using UrlShortener.App.Shared.Dto;

namespace UrlShortener.App.Blazor.Client.Business
{
    /// <summary>
    /// Defines operations related to URL mappings, such as creation, retrieval, deletion, and statistics.
    /// </summary>
    public interface IMappingsService
    {
        /// <summary>
        /// Retrieves all URL mappings for the currently authenticated user.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains a list of <see cref="UrlMappingDto"/>s,
        /// or <c>null</c> if retrieval fails.
        /// </returns>
        Task<List<UrlMappingDto>?> GetMappings();

        /// <summary>
        /// Creates a new shortened URL mapping.
        /// </summary>
        /// <param name="longUrl">The original long URL to shorten.</param>
        /// <param name="name">An optional name to identify the mapping.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains a <see cref="CreateMappingResponseDto"/>
        /// if creation is successful, or <c>null</c> otherwise.
        /// </returns>
        Task<CreateMappingResponseDto?> CreateMapping(string longUrl, string? name = null);

        /// <summary>
        /// Retrieves user statistics, including total mappings and total redirect clicks.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The result contains a <see cref="UserStatsDto"/>, or <c>null</c> if unavailable.
        /// </returns>
        Task<UserStatsDto?> GetStats();

        /// <summary>
        /// Deletes the specified URL mapping.
        /// </summary>
        /// <param name="mapping">The mapping to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The result is <c>true</c> if deletion succeeds; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> DeleteMapping(UrlMappingDto mapping);
    }
}
