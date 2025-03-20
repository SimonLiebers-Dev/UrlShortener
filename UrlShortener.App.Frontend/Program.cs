using Blazorise;
using Blazorise.Icons.FontAwesome;
using Blazorise.Tailwind;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UrlShortener.App.Frontend.Business;

namespace UrlShortener.App.Frontend;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services
            .AddBlazorise()
            .AddTailwindProviders()
            .AddFontAwesomeIcons();

        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
        builder.Services.AddScoped<IMappingsService, MappingsService>();

        builder.Services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();
        builder.Services.AddAuthorizationCore();

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:8080/") });

        await builder.Build().RunAsync();
    }
}
