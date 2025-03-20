using UrlShortener.App.Backend.Models;

namespace UrlShortener.App.Backend.Business
{
    public class IpLookupService(HttpClient HttpClient) : IIpLookupService
    {
        public async Task<IpApiResponse?> GetDataAsync(string? ip)
        {
            if(string.IsNullOrWhiteSpace(ip))
                return null;

            try
            {
                return await HttpClient.GetFromJsonAsync<IpApiResponse>($"http://ip-api.com/json/{ip}");
            }
            catch
            {
                return null;
            }
        }
    }
}
