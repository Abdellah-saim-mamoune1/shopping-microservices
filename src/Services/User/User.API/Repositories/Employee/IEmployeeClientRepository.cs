
using User.API.Dtos;

namespace User.API.Repositories.Employee
{
    public interface IEmployeeClientRepository
    {
        Task<UserGetDto?> GetByIdAsync(int id);
        Task<List<UserGetDto>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
