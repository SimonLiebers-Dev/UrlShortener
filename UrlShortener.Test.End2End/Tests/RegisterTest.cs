using UrlShortener.App.Shared.Models;
using UrlShortener.Test.End2End.Base;
using UrlShortener.Test.End2End.Data;

namespace UrlShortener.Test.End2End.Tests
{
    [TestFixture]
    public class RegisterTest : PlayWrightFullTestBase
    {
        protected override List<User> TestUsers => [];

        [Test]
        public async Task Page_RegisterAndLogin()
        {
            var email = "test@gmail.com";
            var password = "TestPassword";

            await FrontendTest.GotoPageAsync(string.Empty, async page =>
            {
                // Wait for page to render
                await page.WaitForSelectorAsync(TestSelectors.LoginFormSelector);

                // Go to register
                await page.ClickAsync(TestSelectors.RegisterBtnSelector);

                // Wait for register form
                await page.WaitForSelectorAsync(TestSelectors.RegisterFormSelector);

                // Fill form fields
                await page.FillAsync(TestSelectors.RegisterEmailInput, email);
                await page.FillAsync(TestSelectors.RegisterPassword1Input, password);
                await page.FillAsync(TestSelectors.RegisterPassword2Input, password);

                // Submit
                await page.ClickAsync(TestSelectors.RegisterBtnSelector);

                // Wait for login form
                await page.WaitForSelectorAsync(TestSelectors.LoginFormSelector);

                // Fill form
                await page.FillAsync(TestSelectors.LoginEmailInput, email);
                await page.FillAsync(TestSelectors.LoginPasswordInput, password);

                // Click login button
                await page.ClickAsync(TestSelectors.LoginBtnSelector);

                // Loading indicator of mappings/home page
                var loadingIndicator = await page.WaitForSelectorAsync(TestSelectors.MappingsLoadingIndicatorSelector);

                // Assert that home/mappings are rendered
                Assert.That(loadingIndicator, Is.Not.Null);

                // Assert that user is still logged in after page reload
                await page.ReloadAsync();
                loadingIndicator = await page.WaitForSelectorAsync(TestSelectors.MappingsLoadingIndicatorSelector);
                Assert.That(loadingIndicator, Is.Not.Null);
            });
        }
    }
}
