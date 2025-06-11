using NBomber.CSharp;
using System.Net;
using System.Net.Http.Json;
using UrlShortener.App.Shared.Dto;
using UrlShortener.App.Shared.Models;
using UrlShortener.Test.End2End.Base;
using UrlShortener.Test.End2End.Data;

namespace UrlShortener.Test.End2End.Tests.LoadTests
{
    [TestFixture]
    public class RedirectLoadTest : PlayWrightFullTestBase
    {
        private HttpClient _httpClient;

        protected override List<User> TestUsers => TestData.GetDefaultTestUsers();
        protected override List<UrlMapping> TestUrlMappings => TestData.GetDefaultTestUrlMappings();

        [OneTimeSetUp]
        public void Setup()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false // Do not automatically follow redirects
            };

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(BackendTest.Url)
            };
        }

        [Test]
        public void Test_Redirect_WithValidPath()
        {
            // Arrange
            var scenario = Scenario.Create("redirect_valid", async context =>
            {
                var response = await _httpClient.GetAsync("/test");

                return response.StatusCode == HttpStatusCode.Found
                       && response.Headers.Location != null
                       ? Response.Ok(statusCode: response.StatusCode.ToString())
                       : Response.Fail(statusCode: response.StatusCode.ToString());
            })
                .WithoutWarmUp()
                .WithLoadSimulations(Simulation.Inject(rate: 10, interval: TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10)));

            // Act
            var result = NBomberRunner
                .RegisterScenarios(scenario)
                .Run();

            // Assert
            Assert.That(result.AllFailCount, Is.EqualTo(0));
        }

        [Test]
        public void Test_Redirect_WithInvalidPath()
        {
            // Arrange
            var scenario = Scenario.Create("redirect_invalid", async context =>
            {
                var response = await _httpClient.GetAsync("/invalid");

                return response.StatusCode == HttpStatusCode.NotFound
                       ? Response.Ok(statusCode: response.StatusCode.ToString())
                       : Response.Fail(statusCode: response.StatusCode.ToString());
            })
                .WithoutWarmUp()
                .WithLoadSimulations(Simulation.Inject(rate: 10, interval: TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10)));

            // Act
            var result = NBomberRunner
                .RegisterScenarios(scenario)
                .Run();

            // Assert
            Assert.That(result.AllFailCount, Is.EqualTo(0));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}
