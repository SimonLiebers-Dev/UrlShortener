using Microsoft.AspNetCore.Identity.Data;
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
    public class MappingsLoadTest : PlayWrightFullTestBase
    {
        private HttpClient _httpClient;
        protected override List<User> TestUsers => TestData.GetDefaultTestUsers();

        [OneTimeSetUp]
        public async Task SetupAsync()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BackendTest.Url)
            };

            var loginRequest = new LoginRequest
            {
                Email = "test@gmail.com",
                Password = "TestPassword"
            };

            var uri = new Uri("/api/auth/login", UriKind.Relative);
            var response = await _httpClient.PostAsJsonAsync(uri, loginRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Login failed");
            }

            var loginData = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            if (loginData == null || loginData.Token == null)
            {
                throw new Exception("No token received");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginData.Token);
        }

        [Test]
        public void Test_CreateMapping_WithValidRequests()
        {
            // Arrange
            var uri = new Uri("/api/mappings/create", UriKind.Relative);
            var scenario = Scenario.Create("create_mapping", async context =>
            {
                var createRequest = new CreateMappingRequestDto
                {
                    Name = $"Mapping_{context.InvocationNumber}",
                    LongUrl = $"https://example.com/{context.InvocationNumber}"
                };

                var response = await _httpClient.PostAsJsonAsync(uri, createRequest);

                return response.StatusCode switch
                {
                    HttpStatusCode.OK => Response.Ok(statusCode: response.StatusCode.ToString()), // expected case for successful creation
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
        public void Test_GetMappings()
        {
            // Arrange
            var uri = new Uri("/api/mappings/all", UriKind.Relative);
            var scenario = Scenario.Create("get_mappings", async context =>
            {
                var response = await _httpClient.GetAsync(uri);

                return response.StatusCode switch
                {
                    HttpStatusCode.OK => Response.Ok(statusCode: response.StatusCode.ToString()), // expected case
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
        public void Test_GetStats()
        {
            // Arrange
            var uri = new Uri("/api/mappings/stats", UriKind.Relative);
            var scenario = Scenario.Create("get_stats", async context =>
            {
                var response = await _httpClient.GetAsync(uri);

                return response.StatusCode switch
                {
                    HttpStatusCode.OK => Response.Ok(statusCode: response.StatusCode.ToString()), // expected case
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
