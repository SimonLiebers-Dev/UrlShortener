using Microsoft.Playwright;
using UrlShortener.Test.End2End.Base;
using UrlShortener.Test.End2End.Data;

namespace UrlShortener.Test.End2End.Tests
{
    [TestFixture]
    public class FlowTest : PlayWrightFullTestBase
    {
        [Test]
        public async Task Login_Create_ShowDetails()
        {
            await FrontendTest.GotoPageAsync(string.Empty, async page =>
            {
                // Wait for login form
                await page.WaitForSelectorAsync(TestSelectors.LoginFormSelector);

                // Fill login form with credentials
                await page.FillAsync(TestSelectors.LoginEmailInput, "test@gmail.com");
                await page.FillAsync(TestSelectors.LoginPasswordInput, "TestPassword");

                // Click to login
                await page.ClickAsync(TestSelectors.LoginBtnSelector);

                // Wait for inputs
                await page.WaitForSelectorAsync(TestSelectors.CreateMappingNameInputSelector);
                await page.WaitForSelectorAsync(TestSelectors.CreateMappingUrlInputSelector);

                // Input test data
                await page.FillAsync(TestSelectors.CreateMappingNameInputSelector, "Test");
                await page.FillAsync(TestSelectors.CreateMappingUrlInputSelector, "https://www.test.com");

                // Click button to create new mapping
                await page.ClickAsync(TestSelectors.MappingCreateBtnSelector);

                // Wait for mapping card
                await page.WaitForSelectorAsync(TestSelectors.MappingCardSelector);

                // Query all available mappings
                var cards = await page.QuerySelectorAllAsync(TestSelectors.MappingCardSelector);

                // Find newly created mapping in cards
                IElementHandle? targetCard = null;
                foreach (var card in cards)
                {
                    var innerHtml = await card.InnerHTMLAsync();
                    if (innerHtml.Contains("https://www.test.com"))
                    {
                        targetCard = card;
                        break;
                    }
                }

                // Assert that card exists
                Assert.That(targetCard, Is.Not.Null);

                // Wait for details button in card
                var detailsBtn = await targetCard.WaitForSelectorAsync(TestSelectors.MappingDetailsBtnSelector);

                // Assert that button exists
                Assert.That(detailsBtn, Is.Not.Null);

                // Click details button to show modal
                await detailsBtn.ClickAsync();

                // Wait for modal to render
                var modal = await page.WaitForSelectorAsync(TestSelectors.MappingDetailsModalSelector);

                // Assert that modal is rendered
                Assert.That(modal, Is.Not.Null);

                // Find close button in modal
                var closeBtn = await modal.WaitForSelectorAsync("button");

                // Assert that close button exists
                Assert.That(closeBtn, Is.Not.Null);

                // Click close button
                await closeBtn.ClickAsync();
            });
        }
    }
}
