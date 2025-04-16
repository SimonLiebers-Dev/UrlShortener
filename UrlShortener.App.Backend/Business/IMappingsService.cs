using UrlShortener.App.Shared.Models;

namespace UrlShortener.App.Backend.Business
{
    /// <summary>
    /// Service to manage URL mappings
    /// </summary>
    public interface IMappingsService
    {
        /// <summary>
        /// Create a new mapping for a long URL
        /// </summary>
        /// <param name="longUrl">Long url</param>
        /// <param name="name">Name of mapping</param>
        /// <param name="email">Email of user</param>
        /// <returns></returns>
        Task<UrlMapping?> CreateMapping(string longUrl, string name, string email);

        /// <summary>
        /// Get a mapping by its short url path
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>UrlMapping with matching path</returns>
        Task<UrlMapping?> GetMappingByPath(string path);

        /// <summary>
        /// Get all mappings of a user
        /// </summary>
        /// <param name="email">Email of the user</param>
        /// <returns>All mappings found</returns>
        Task<List<UrlMapping>?> GetMappingsByUser(string email);

        /// <summary>
        /// Delete a mapping of a user by its id
        /// </summary>
        /// <param name="email">Email of the user</param>
        /// <param name="mappingId">Id of the mapping</param>
        /// <returns>True, if it was successfully deleted</returns>
        Task<bool> DeleteMapping(string email, int mappingId);
    }
}
