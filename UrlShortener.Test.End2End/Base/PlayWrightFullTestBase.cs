using SoloX.CodeQuality.Playwright;
using UrlShortener.App.Shared.Models;
using UrlShortener.Test.End2End.Data;

namespace UrlShortener.Test.End2End.Base
{
    [NonParallelizable]
    public class PlayWrightFullTestBase : PlayWrightTestBase
    {
        protected IPlaywrightTest FrontendTest;
        protected IPlaywrightTest BackendTest;

        protected override List<User> TestUsers => TestData.GetDefaultTestUsers();

        [OneTimeSetUp]
        public virtual async Task OneTimeSetup()
        {
            // Create backend builder
            var backendBuilder = CreateBackendBuilder();

            // Run backend
            BackendTest = await backendBuilder
                .BuildAsync()
                .ConfigureAwait(true);

            Console.WriteLine($"Backend running on: {BackendTest.Url}");

            // Create frontend builder
            var frontendBuilder = CreateFrontendBuilder(BackendTest);

            // Run frontend
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
    }
}
