using Authentication.API.Dtos;
using Authentication.API.Entities;

namespace Authentication.API.Repositories
{
    public interface IAuthenticationRepository
    {
        public Task<User?> GetUserByAccountAsync(string account);
        public Task<User> CreateClientAsync(User user, string password);
        public Task<User> CreateEmployeeAsync(User user, string password);
        public Task<bool> ExistsByEmailAsync(string Account);
        public bool VerifyPassword(User user, string password);
        public Task<(string accessToken, string refreshToken, DateTime expires)> GenerateTokensAsync(User user);
        public Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    }
}
