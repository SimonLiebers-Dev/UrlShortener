using UrlShortener.Test.End2End.Base;
using UrlShortener.Test.End2End.Data;

namespace UrlShortener.Test.End2End.Tests
{
    [TestFixture]
    public class LoginTest : PlayWrightFullTestBase
    {
        [Test]
        public async Task Page_CanSwitchBetweenLoginAndRegister()
        {
            await FrontendTest.GotoPageAsync(string.Empty, async page =>
            {
                // Wait for login form
                await page.WaitForSelectorAsync(TestSelectors.LoginFormSelector);

                // Check if login is displayed
                var pageTitle = await page.TitleAsync();
                Assert.That(pageTitle, Is.EqualTo(TestSelectors.LoginExpectedTitle));

                // Click on register button
                await page.ClickAsync(TestSelectors.RegisterBtnSelector);
                await page.WaitForSelectorAsync(TestSelectors.RegisterFormSelector);

                // Check if register is displayed
                pageTitle = await page.TitleAsync();
                Assert.That(pageTitle, Is.EqualTo(TestSelectors.RegisterExpectedTitle));

                // Click on login button
                await page.ClickAsync(TestSelectors.LoginBtnSelector);
                await page.WaitForSelectorAsync(TestSelectors.LoginFormSelector);

                // Check if login is displayed
                pageTitle = await page.TitleAsync();
                Assert.That(pageTitle, Is.EqualTo(TestSelectors.LoginExpectedTitle));
            });
        }

        [Test]
        public async Task Login_ValidCredentials_RedirectsToMappings()
        {
            await FrontendTest.GotoPageAsync(string.Empty, async page =>
            {
                // Wait for render
                await page.WaitForSelectorAsync(TestSelectors.LoginFormSelector);

                // Check if login displayed
                var loginTitle = await page.TitleAsync();
                Assert.That(loginTitle, Is.EqualTo("Login"));

                // Fill login form with invalid credentials
                await page.FillAsync(TestSelectors.LoginEmailInput, "test@gmail.com");
                await page.FillAsync(TestSelectors.LoginPasswordInput, "TestPassword");

                // Submit login
                await page.ClickAsync(TestSelectors.LoginBtnSelector);

                // Wait for mappings and assert
                var mappingsWrapper = await page.WaitForSelectorAsync(TestSelectors.MappingsLoadingIndicatorSelector);
                Assert.That(mappingsWrapper, Is.Not.Null);
            });
        }

        [Test]
        public async Task Login_InvalidCredentials_DisplaysError()
        {
            await FrontendTest.GotoPageAsync(string.Empty, async page =>
            {
                // Wait for render
                var body = page.Locator("body");
                await body.WaitForAsync();

                // Check if login displayed
                var loginTitle = await page.TitleAsync();
                Assert.That(loginTitle, Is.EqualTo("Login"));

                // Fill login form with invalid credentials
                await page.FillAsync(TestSelectors.LoginEmailInput, "test@gmail.com");
                await page.FillAsync(TestSelectors.LoginPasswordInput, "InvalidPassword");

                // Submit login
                await page.ClickAsync(TestSelectors.LoginBtnSelector);

                // Wait for error message and assert
                var snackbar = await page.WaitForSelectorAsync(".snackbar-show.snackbar-danger");
                Assert.That(snackbar, Is.Not.Null);
            });
        }
    }
}
