namespace Authentication.API.Services
{
    public interface ITokenService
    {
        public string CreateToken(string Id, string Role);
        public string GenerateRefreshToken();
 
    }
}
