using SoloX.CodeQuality.Playwright;
using Microsoft.EntityFrameworkCore;
using UrlShortener.App.Backend;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.App.Shared.Models;
using UrlShortener.App.Blazor.Client.Business;
using UrlShortener.App.Blazor.Client.Api;
using UrlShortener.Test.End2End.Data;

namespace UrlShortener.Test.End2End.Base
{
    [NonParallelizable]
    public class PlayWrightTestBase
    {
        private bool RunHeadless => IsRunningInCI() ? true : Headless;

        protected IPlaywrightTest FrontendTest;
        protected IPlaywrightTest BackendTest;
        protected virtual bool Headless => false;
        protected virtual List<User> TestUsers => TestData.GetDefaultTestUsers();

        [OneTimeSetUp]
        public virtual async Task OneTimeSetup()
        {
            // Create and run backend
            var backendBuilder = PlaywrightTestBuilder.Create()
                .WithLocalHost(localHostBuilder =>
                {
                    localHostBuilder
                        .UseApplication<Program>()
                        .UseWebHostBuilder(webHostBuilder =>
                        {
                            webHostBuilder.ConfigureServices(services =>
                            {
                                services.AddEntityFrameworkInMemoryDatabase();

                                var provider = services
                                    .AddEntityFrameworkInMemoryDatabase()
                                    .BuildServiceProvider();

                                services.AddDbContext<AppDbContext>(options =>
                                {
                                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                                    options.UseInternalServiceProvider(provider);
                                });

                                var sp = services.BuildServiceProvider();

                                using var scope = sp.CreateScope();

                                var scopedServices = scope.ServiceProvider;
                                var db = scopedServices.GetRequiredService<AppDbContext>();

                                db.Database.EnsureCreated();

                                db.Users.AddRange(TestUsers);
                                db.SaveChanges();
                            });
                        })
                        .UseHttps(false);
                });

            backendBuilder = backendBuilder
                .WithPlaywrightOptions(opt =>
                {
                    opt.Headless = true; // Backend always headless
                });

            BackendTest = await backendBuilder
                .BuildAsync()
                .ConfigureAwait(true);

            Console.WriteLine($"Backend running on: {BackendTest.Url}");

            // Create and run frontend
            var frontendBuilder = PlaywrightTestBuilder.Create()
                .WithLocalHost(localHostBuilder =>
                {
                    localHostBuilder
                        .UseApplication<App.Blazor.Program>()
                        .UseWebHostBuilder(webHostBuilder =>
                        {
                            webHostBuilder.ConfigureServices(services =>
                            {
                                services.AddHttpClient<IAuthApi, AuthApi>().ConfigureHttpClient(client =>
                                {
                                    client.BaseAddress = new Uri(BackendTest.Url);
                                });
                                services.AddHttpClient<IMappingsService, MappingsService>().ConfigureHttpClient(client =>
                                {
                                    client.BaseAddress = new Uri(BackendTest.Url);
                                });
                            });
                        })
                        .UseHttps(false);
                });

            frontendBuilder = frontendBuilder
                .WithPlaywrightOptions(opt =>
                {
                    opt.Headless = RunHeadless;
                })
                .WithPlaywrightNewContextOptions(opt =>
                {
                    opt.ViewportSize = new Microsoft.Playwright.ViewportSize() { Width = 1600, Height = 1600 };
                });


            FrontendTest = await frontendBuilder
                .BuildAsync(Browser.Chromium, Devices.DesktopChrome)
                .ConfigureAwait(true);

            Console.WriteLine($"Frontend running on: {FrontendTest.Url}");
        }

        [OneTimeTearDown]
        public virtual async Task OneTimeTearDown()
        {
            await BackendTest.DisposeAsync();
            await FrontendTest.DisposeAsync();
        }

        private static bool IsRunningInCI()
        {
            bool isGitHubActions = bool.TryParse(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"), out bool githubAction) && githubAction;
            bool isCI = bool.TryParse(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"), out bool ci) && ci;

            return isGitHubActions || isCI;
        }
    }
}
