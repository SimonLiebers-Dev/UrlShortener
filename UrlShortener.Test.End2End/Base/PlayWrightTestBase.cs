using SoloX.CodeQuality.Playwright;
using Microsoft.EntityFrameworkCore;
using UrlShortener.App.Backend;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.App.Shared.Models;
using UrlShortener.App.Blazor.Client.Business;
using UrlShortener.App.Blazor.Client.Api;
using UrlShortener.App.Backend.Utils;

namespace UrlShortener.Test.End2End.Base
{
    public class PlayWrightTestBase
    {
        protected IPlaywrightTest FrontendTest;
        protected IPlaywrightTest BackendTest;
        protected virtual bool Headless => true;

        [SetUp]
        public async Task Setup()
        {
            Console.WriteLine($"GITHUB_ACTIONS: {Environment.GetEnvironmentVariable("GITHUB_ACTIONS")}");
            Console.WriteLine($"CI: {Environment.GetEnvironmentVariable("CI")}");

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
                        });
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
                        });
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

            FrontendTest = await builder.BuildAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await BackendTest.DisposeAsync();
            await FrontendTest.DisposeAsync();
        }
    }
}
