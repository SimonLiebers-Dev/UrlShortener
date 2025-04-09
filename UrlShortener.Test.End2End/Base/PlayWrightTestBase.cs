using SoloX.CodeQuality.Playwright;
using Microsoft.EntityFrameworkCore;
using UrlShortener.App.Backend;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.App.Shared.Models;
using UrlShortener.App.Blazor.Client.Business;
using UrlShortener.App.Blazor.Client.Api;

namespace UrlShortener.Test.End2End.Base
{
    [NonParallelizable]
    public class PlayWrightTestBase
    {
        /// <summary>
        /// Tests are always run headless when running in ci pipeline, otherwise Headless value is used
        /// </summary>
        protected bool RunHeadless => IsRunningInCI() ? true : Headless;

        /// <summary>
        /// Define if test should be run in headless mode
        /// </summary>
        protected virtual bool Headless => false;

        /// <summary>
        /// Test users to be used in tests
        /// </summary>
        protected virtual List<User> TestUsers => [];

        /// <summary>
        /// Test mappings to be used in tests
        /// </summary>
        protected virtual List<UrlMapping> TestUrlMappings => [];

        /// <summary>
        /// Checks if the tests are running in a CI environment
        /// </summary>
        /// <returns></returns>
        private static bool IsRunningInCI()
        {
            bool isGitHubActions = bool.TryParse(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"), out bool githubAction) && githubAction;
            bool isCI = bool.TryParse(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"), out bool ci) && ci;

            return isGitHubActions || isCI;
        }

        /// <summary>
        /// Create the backend test builder
        /// </summary>
        /// <returns>IPlaywrightTestBuilder?</returns>
        protected IPlaywrightTestBuilder CreateBackendBuilder()
        {
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
                                db.UrlMappings.AddRange(TestUrlMappings);
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

            return backendBuilder;
        }

        /// <summary>
        /// Create the frontend test builder
        /// </summary>
        /// <param name="backendTest">Backend test</param>
        /// <returns>IPlaywrightTestBuilder?</returns>
        protected IPlaywrightTestBuilder CreateFrontendBuilder(IPlaywrightTest backendTest)
        {
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
                                    client.BaseAddress = new Uri(backendTest.Url);
                                });
                                services.AddHttpClient<IMappingsService, MappingsService>().ConfigureHttpClient(client =>
                                {
                                    client.BaseAddress = new Uri(backendTest.Url);
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

            return frontendBuilder;
        }
    }
}
