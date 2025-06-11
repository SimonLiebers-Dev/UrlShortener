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
    public class RegisterLoadTest : PlayWrightFullTestBase
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
        public void Test_Register_WithDistinctEmails()
        {
            // Arrange
            var scenario = Scenario.Create("register_valid", async context =>
            {
                var uniqueId = Guid.NewGuid().ToString("N")[..8];
                var registerRequest = new RegisterRequestDto
                {
                    Email = $"{context.InvocationNumber}{uniqueId}@gmail.com",
                    Password = "TestPassword"
                };

                var response = await _httpClient.PostAsJsonAsync("/api/auth/register", registerRequest);

                return response.StatusCode switch
                {
                    HttpStatusCode.OK => Response.Ok(statusCode: response.StatusCode.ToString()), // expected case for successful registration
                    HttpStatusCode.TooManyRequests => Response.Ok(statusCode: response.StatusCode.ToString()), // expected case because of rate limiting
                    _ => Response.Fail(statusCode: response.StatusCode.ToString())
                };
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
        public void Test_Register_WithExistingEmails()
        {
            // Arrange
            var scenario = Scenario.Create("register_conflict", async context =>
            {
                var registerRequest = new RegisterRequestDto
                {
                    Email = $"test@gmail.com",
                    Password = "TestPassword"
                };

                var response = await _httpClient.PostAsJsonAsync("/api/auth/register", registerRequest);

                return response.StatusCode switch
                {
                    HttpStatusCode.Conflict => Response.Ok(statusCode: response.StatusCode.ToString()), // expected case for successful registration
                    HttpStatusCode.TooManyRequests => Response.Ok(statusCode: response.StatusCode.ToString()), // expected case because of rate limiting
                    _ => Response.Fail(statusCode: response.StatusCode.ToString())
                };
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
