using User.API.Dtos;

namespace User.API.Services.Employee
{
    public interface IEmployeeClientService
    {
        public Task<ApiResponseDto<UserGetDto>> GetByIdAsync(int id);
        public Task<ApiResponseDto<IEnumerable<UserGetDto>>> GetAllAsync();
        public Task<ApiResponseDto<object>> DeleteAsync(int id);

    }

}
