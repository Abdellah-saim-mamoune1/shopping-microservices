using Authentication.API.Dtos;

namespace Authentication.API.Services
{
    public interface IAuthenticationService
    {
        Task<ApiResponseDto<AuthResponseDto>> LoginAsync(LoginDto request);

        Task<ApiResponseDto<AuthResponseDto>> RegisterClientAsync(ClientRegistrationDto request);

        Task<ApiResponseDto<AuthResponseDto>> RegisterEmployeeAsync(EmployeeRegistrationDto request);

        Task<ApiResponseDto<AuthResponseDto>> RefreshTokensAsync(int userId, string refreshToken);
    }
}
