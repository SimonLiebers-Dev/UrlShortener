using SoloX.CodeQuality.Playwright;
using UrlShortener.App.Blazor;

namespace UrlShortener.Test.E2E.Utils
{
    internal static class PlaywrightUtils
    {
        public static async Task<IPlaywrightTest> GetPlaywrightTestAsync(bool headless = true)
        {
            if (IsRunningInCI())
                headless = false;

            var builder = PlaywrightTestBuilder.Create()
                // Tells that we run a local host.
                .WithLocalHost(localHostBuilder =>
                {
                    localHostBuilder
                        // It tells that we use a local application and we provide a type
                        // defined in the application assembly entry point (like Program, App
                        // or any type defined in the host assembly).
                        .UseApplication<Program>()
                        // Since the test builder is using the WebApplicationFactory, we can
                        // use the IWebHostBuilder.
                        .UseWebHostBuilder(webHostBuilder =>
                        {
                            // Specify some settings
                            webHostBuilder.UseSetting("MySettingKey", "MySettingValue");

                            // Specify some service mocks
                            webHostBuilder.ConfigureServices(services =>
                            {
                                // Replace services with mocks
                            });
                        });
                });

            builder = builder
                // Allows you to set up Playwright options.
                .WithPlaywrightOptions(opt =>
                {
                    // Tells that we want the browser to be displayed on the screen.
                    opt.Headless = headless;
                })
                // Allows you to set up Playwright New Context options.
                .WithPlaywrightNewContextOptions(opt =>
                {
                    // Tells that the viewport size will be 1000x800
                    opt.ViewportSize = new Microsoft.Playwright.ViewportSize() { Width = 800, Height = 500 };
                });

            return await builder.BuildAsync();
        }

        private static bool IsRunningInCI()
        {
            // Check for common CI environment variables
            return Environment.GetEnvironmentVariable("CI") != null;
        }
    }
}
