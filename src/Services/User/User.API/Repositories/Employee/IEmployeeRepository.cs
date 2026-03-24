using User.API.Dtos;

namespace User.API.Repositories.Employee
{
    public interface IEmployeeRepository
    {
        Task<UserGetDto?> GetAsync(int id);
        Task UpdateAsync(int Id,UserUpdateDto user);
        Task<List<UserGetDto>> GetAllAsync();
    }
}
