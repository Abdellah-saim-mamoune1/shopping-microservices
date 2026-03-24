using User.API.Dtos;

namespace User.API.Services.Client
{
    public interface IClientService
    {
        public Task<ApiResponseDto<UserGetDto>> GetByIdAsync(int id);
        public Task<ApiResponseDto<object>> UpdateByIdAsync(int id, UserUpdateDto user);


    }   

}
