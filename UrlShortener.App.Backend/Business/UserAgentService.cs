using UrlShortener.App.Backend.Models;

namespace UrlShortener.App.Backend.Business
{
    /// <summary>
    /// Service for retrieving parsed user agent information using an external API.
    /// Implements <see cref="IUserAgentService"/>.
    /// </summary>
    internal class UserAgentService(HttpClient httpClient) : IUserAgentService
    {
        /// <inheritdoc />
        public async Task<UserAgentApiResponse?> GetUserAgentAsync(string userAgent)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<UserAgentApiResponse>($"https://api.apicagent.com/?ua={userAgent}");
            }
            catch
            {
                return null;
            }
        }
    }
}
