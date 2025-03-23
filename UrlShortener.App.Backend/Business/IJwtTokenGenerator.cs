namespace UrlShortener.App.Backend.Business
{
    internal interface IJwtTokenGenerator
    {
        public string GenerateToken(string email);
    }
}
