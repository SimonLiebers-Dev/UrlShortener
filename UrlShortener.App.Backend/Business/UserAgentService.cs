using UrlShortener.App.Backend.Models;

namespace UrlShortener.App.Backend.Business
{
    /// <summary>
    /// Service to retrieve detailed information about a user agent
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
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
