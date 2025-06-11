using Microsoft.AspNetCore.Identity.Data;
using NBomber.CSharp;
using System.Net;
using System.Net.Http.Json;
using UrlShortener.App.Shared.Models;
using UrlShortener.Test.End2End.Base;
using UrlShortener.Test.End2End.Data;

namespace UrlShortener.Test.End2End.Tests.LoadTests
{
    [TestFixture]
    public class LoginLoadTest : PlayWrightFullTestBase
    {
        private HttpClient _httpClient;
        protected override List<User> TestUsers => TestData.GetDefaultTestUsers();

        [OneTimeSetUp]
        public void Setup()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BackendTest.Url)
            };
        }

        [Test]
        public void Test_Login_WithValidCredentials()
        {
            // Arrange
            var scenario = Scenario.Create("login_valid", async context =>
            {
                var loginRequest = new LoginRequest
                {
                    Email = "test@gmail.com",
                    Password = "TestPassword"
                };

                var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginRequest);

                return response.StatusCode switch
                {
                    HttpStatusCode.OK => Response.Ok(statusCode: response.StatusCode.ToString()), // expected case for valid login
                    HttpStatusCode.TooManyRequests => Response.Ok(statusCode: response.StatusCode.ToString()), // expected case because of rate limiting
                    _ => Response.Fail(statusCode: response.StatusCode.ToString())
                };
            })
                .WithoutWarmUp()
                .WithLoadSimulations(Simulation.Inject(rate: 30, interval: TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10)));

            // Act
            var result = NBomberRunner
                .RegisterScenarios(scenario)
                .Run();

            // Assert
            Assert.That(result.AllFailCount, Is.EqualTo(0));
        }

        [Test]
        public void Test_Login_WithInvalidCredentials()
        {
            // Arrange
            var scenario = Scenario.Create("login_invalid", async context =>
            {
                var loginRequest = new LoginRequest
                {
                    Email = "invalid@gmail.com",
                    Password = "TestPassword"
                };

                var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginRequest);

                return response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => Response.Ok(statusCode: response.StatusCode.ToString()), // expected case for invalid login
                    HttpStatusCode.TooManyRequests => Response.Ok(statusCode: response.StatusCode.ToString()), // expected case because of rate limiting
                    _ => Response.Fail(statusCode: response.StatusCode.ToString())
                };
            })
                .WithoutWarmUp()
                .WithLoadSimulations(Simulation.Inject(rate: 30, interval: TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10)));

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
