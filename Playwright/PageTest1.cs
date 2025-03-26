using Playwright.Infrastructure;

namespace Playwright
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class PageTest1 : BlazorTest
    {
        [Test]
        public async Task Test()
        {
            //await Page.GotoAsync(RootUri.AbsoluteUri);

            //Assert.That(await Page.TitleAsync(), Is.EqualTo("Home"));
        }
    }
}
