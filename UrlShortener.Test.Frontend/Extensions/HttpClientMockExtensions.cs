using Moq;
using System.Net.Http.Json;
using System.Net;
using Moq.Protected;

namespace UrlShortener.Test.Frontend.Extensions
{
    internal static class HttpClientMockExtensions
    {
        public static void SetupRequest<T>(this Mock<HttpMessageHandler> mock, HttpMethod method, string url, T responseContent, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            mock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == method && req.RequestUri!.ToString().EndsWith(url)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = JsonContent.Create(responseContent)
                });
        }

        public static void SetupRequest(this Mock<HttpMessageHandler> mock, HttpMethod method, string url, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            mock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == method && req.RequestUri!.ToString().EndsWith(url)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                });
        }
    }
}
