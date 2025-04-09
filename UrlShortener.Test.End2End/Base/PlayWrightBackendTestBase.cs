using SoloX.CodeQuality.Playwright;

namespace UrlShortener.Test.End2End.Base
{
    [NonParallelizable]
    public class PlayWrightBackendTestBase : PlayWrightTestBase
    {
        protected IPlaywrightTest BackendTest;

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
        }

        [OneTimeTearDown]
        public virtual async Task OneTimeTearDown()
        {
            await BackendTest.DisposeAsync();
        }
    }
}
