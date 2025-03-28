using UrlShortener.Test.End2End.Base;

namespace UrlShortener.Test.End2End.Tests
{
    [TestFixture]
    public class LoginTest : PlayWrightTestBase
    {
        [Test]
        public async Task Login_ValidCredentials_RedirectToHome()
        {
            await FrontendTest.GotoPageAsync(string.Empty, async page =>
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
