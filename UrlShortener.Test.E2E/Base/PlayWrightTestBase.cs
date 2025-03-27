using SoloX.CodeQuality.Playwright;
using Microsoft.EntityFrameworkCore;
using UrlShortener.App.Backend;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.App.Shared.Models;
using UrlShortener.App.Backend.Utils;
using UrlShortener.App.Blazor.Client.Business;
using UrlShortener.App.Blazor.Client.Api;

namespace UrlShortener.Test.E2E.Base
{
    public class PlayWrightTestBase
    {
        protected IPlaywrightTest PlayWrightTest;
        protected IPlaywrightTest BackendTest;
        protected virtual bool Headless => false;

        [SetUp]
        public async Task Setup()
        {
            var backendBuilder = PlaywrightTestBuilder.Create()
                // Tells that we run a local host.
                .WithLocalHost(localHostBuilder =>
                {
                    localHostBuilder
                        .UseApplication<Program>()
                        .UseWebHostBuilder(webHostBuilder =>
                        {
                            // Specify some service mocks
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

                                var passwordSalt = PasswordUtils.GenerateSalt();
                                var testUser = new User()
                                {
                                    Email = "test@gmail.com",
                                    PasswordHash = PasswordUtils.HashPassword("TestPassword", passwordSalt),
                                    Salt = passwordSalt
                                };

                                db.Users.Add(testUser);
                                db.SaveChanges();
                            });
                        })
                        .UseHttps();
                });

            backendBuilder = backendBuilder
                .WithPlaywrightOptions(opt =>
                {
                    opt.Headless = true; // Backend always headless
                });

            BackendTest = await backendBuilder.BuildAsync();

            var builder = PlaywrightTestBuilder.Create()
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
                        .UseHttps();
                });

            builder = builder
                .WithPlaywrightOptions(opt =>
                {
                    opt.Headless = Headless;
                })
                .WithPlaywrightNewContextOptions(opt =>
                {
                    opt.ViewportSize = new Microsoft.Playwright.ViewportSize() { Width = 800, Height = 500 };
                });

            PlayWrightTest = await builder.BuildAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await BackendTest.DisposeAsync();
            await PlayWrightTest.DisposeAsync();
        }
    }
}
