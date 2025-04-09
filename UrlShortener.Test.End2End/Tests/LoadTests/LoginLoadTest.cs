using Microsoft.AspNetCore.Identity.Data;
using NBomber.CSharp;
using System.Net.Http.Json;
using UrlShortener.App.Shared.Models;
using UrlShortener.Test.End2End.Base;
using UrlShortener.Test.End2End.Data;

namespace UrlShortener.Test.End2End.Tests.LoadTests
{
    [TestFixture]
    public class LoginLoadTest : PlayWrightBackendTestBase
    {
        private HttpClient _httpClient;
        protected override List<User> TestUsers => TestData.GetDefaultTestUsers();

        [OneTimeSetUp]
        public void Setup()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(BackendTest.Url)
            };
        }

        [Test]
        public void TestLoginEndpoint()
        {
            // Arrange
            var scenario = Scenario.Create("login_scenario", async context =>
            {
                var loginRequest = new LoginRequest
                {
                    Email = "test@gmail.com",
                    Password = "TestPassword"
                };

                var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginRequest);
                return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
            })
                .WithoutWarmUp()
                .WithLoadSimulations(Simulation.Inject(rate: 10, interval: TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10)));

            // Act
            var result = NBomberRunner
                .RegisterScenarios(scenario)
                .Run();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.AllRequestCount, Is.GreaterThan(0));
                Assert.That(result.AllOkCount, Is.GreaterThan(0));
                Assert.That(result.AllFailCount, Is.EqualTo(0));
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}
