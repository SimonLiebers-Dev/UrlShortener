using UrlShortener.App.Backend.Models;

namespace UrlShortener.App.Backend.Business
{
    public class UserAgentService(HttpClient httpClient) : IUserAgentService
    {
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
