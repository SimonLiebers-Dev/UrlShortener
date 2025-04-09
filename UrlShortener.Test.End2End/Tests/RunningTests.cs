using UrlShortener.Test.End2End.Base;

namespace UrlShortener.Test.End2End.Tests
{
    [TestFixture]
    public class RunningTests : PlayWrightFullTestBase
    {
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void Setup()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(BackendTest.Url)
            };
        }

        [Test]
        public async Task Frontend_UpAndRunning()
        {
            await FrontendTest.GotoPageAsync(string.Empty, async page =>
            {
                var body = page.Locator("body");
                await body.WaitForAsync();
                Assert.Pass();
            });
        }

        [Test]
        public async Task Backend_UpAndRunning()
        {
            var response = await _httpClient.GetAsync("/api/health");
            Assert.That(response.IsSuccessStatusCode, Is.True);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}
