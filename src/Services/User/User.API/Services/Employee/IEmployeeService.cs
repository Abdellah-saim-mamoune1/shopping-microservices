using User.API.Dtos;

namespace User.API.Services.Employee
{
    public interface IEmployeeService
    {
        public Task<ApiResponseDto<UserGetDto>> GetByIdAsync(int id);
        public Task<ApiResponseDto<object>> UpdateAsync(int id, UserUpdateDto user);
        public Task<ApiResponseDto<List<UserGetDto>>> GetAllAsync();
    }
}
