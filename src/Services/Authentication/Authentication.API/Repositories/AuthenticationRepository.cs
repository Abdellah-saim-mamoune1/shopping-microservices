using Authentication.API.Data;
using Authentication.API.Entities;
using Authentication.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Authentication.API.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly AppDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<User> _passwordHasher;
   

    public AuthenticationRepository(
        AppDbContext db,
        ITokenService tokenService,
        IPasswordHasher<User> passwordHasher)
    {
        _db = db;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    private string HashRefreshToken(string token)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public async Task<User?> GetUserByAccountAsync(string account)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Account == account);
    }

    public async Task<User> CreateClientAsync(User user, string password)
    {
        user.HashedPassword = _passwordHasher.HashPassword(user, password);

        var refreshToken = _tokenService.GenerateRefreshToken();
        user.HashedRefreshToken = HashRefreshToken(refreshToken);
        user.RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(7);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return user;
    }

    public async Task<User> CreateEmployeeAsync(User user, string password)
    {
        user.HashedPassword = _passwordHasher.HashPassword(user, password);

        var refreshToken = _tokenService.GenerateRefreshToken();
        user.HashedRefreshToken = HashRefreshToken(refreshToken);
        user.RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(7);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return user;
    }


    public Task<bool> ExistsByEmailAsync(string Account)
    {
        return _db.Users.AnyAsync(u => u.Account == Account);
    }

    public bool VerifyPassword(User user, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);
        return result != PasswordVerificationResult.Failed;
    }

    public async Task<(string accessToken, string refreshToken, DateTime expires)> GenerateTokensAsync(User user)
    {
        var accessToken = _tokenService.CreateToken(user.Id.ToString(), user.Type);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.HashedRefreshToken = HashRefreshToken(refreshToken);
        user.RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(7);

        await _db.SaveChangesAsync();

        return (accessToken, refreshToken, user.RefreshTokenExpirationDate);
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        var hashedToken = HashRefreshToken(refreshToken);
        var user = await _db.Users.FirstOrDefaultAsync(u =>
            u.HashedRefreshToken == hashedToken &&
            u.RefreshTokenExpirationDate > DateTime.UtcNow);

        return user;
    }
}