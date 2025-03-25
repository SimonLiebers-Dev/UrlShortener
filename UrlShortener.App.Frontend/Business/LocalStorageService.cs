using Microsoft.JSInterop;

namespace UrlShortener.App.Frontend.Business
{
    internal class LocalStorageService(IJSRuntime JsRuntime) : ILocalStorageService
    {
        public async Task<string?> GetItemAsync(string key)
        {
            var result = await JsRuntime.InvokeAsync<string>("localStorageInterop.getItem", key);
            if (string.IsNullOrEmpty(result))
                return null;

            return result;
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
