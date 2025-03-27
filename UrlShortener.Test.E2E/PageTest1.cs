using UrlShortener.Test.E2E.Utils;

namespace UrlShortener.Test.E2E
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class PageTest1
    {
        [Test]
        public async Task Test()
        {
            var test = await PlaywrightUtils.GetPlaywrightTestAsync(false);

            await test.GotoPageAsync(string.Empty, async page =>
            {
                var body = page.Locator("body");
                await body.WaitForAsync();

                var title = await page.TitleAsync();

                Assert.That(title, Is.EqualTo("Login"));
            });
        }
    }
}
