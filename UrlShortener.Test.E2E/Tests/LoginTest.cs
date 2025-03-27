using UrlShortener.Test.E2E.Base;

namespace UrlShortener.Test.E2E.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class LoginTest : PlayWrightTestBase
    {
        protected override bool Headless => true;

        [Test]
        public async Task Test()
        {
            await PlayWrightTest.GotoPageAsync(string.Empty, async page =>
            {
                var body = page.Locator("body");
                await body.WaitForAsync();

                var loginTitle = await page.TitleAsync();
                Assert.That(loginTitle, Is.EqualTo("Login"));

                await page.FillAsync("#login-email-input", "test@gmail.com");
                await page.FillAsync("#login-password-input", "TestPassword");

                await page.ClickAsync("#login-btn");

                await page.WaitForSelectorAsync("#mappings-loading-indicator");

                var homeTitle = await page.TitleAsync();
                Assert.That(homeTitle, Is.EqualTo("UrlShortener"));
            });
        }
    }
}
