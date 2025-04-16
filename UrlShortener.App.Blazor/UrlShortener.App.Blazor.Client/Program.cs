using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace UrlShortener.App.Blazor.Client;

/// <summary>
/// Entry point for the Blazor WebAssembly client application.
/// Sets up the host and launches the app in the browser.
/// </summary>
public static class Program
{
    /// <summary>
    /// Configures and runs the Blazor WebAssembly host.
    /// </summary>
    /// <param name="args">Command-line arguments passed to the WebAssembly application.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        await builder.Build().RunAsync();
    }
}
