using Microsoft.JSInterop;

namespace UrlShortener.App.Frontend.Business
{
    public class LocalStorageService(IJSRuntime JsRuntime) : ILocalStorageService
    {
        public async Task<string> GetItemAsync(string key)
        {
            return await JsRuntime.InvokeAsync<string>("localStorageInterop.getItem", key);
        }

        public async Task RemoveItemAsync(string key)
        {
            await JsRuntime.InvokeVoidAsync("localStorageInterop.removeItem", key);
        }

        public async Task SetItemAsync(string key, string value)
        {
            await JsRuntime.InvokeVoidAsync("localStorageInterop.setItem", key, value);
        }
    }
}
