namespace UrlShortener.App.Backend.Business
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(string email);
    }
}
