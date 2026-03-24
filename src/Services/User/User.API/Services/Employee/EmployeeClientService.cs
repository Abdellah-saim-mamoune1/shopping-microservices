using User.API.Dtos;
using User.API.Repositories.Employee;


namespace User.API.Services.Employee
{
    public class EmployeeClientService : IEmployeeClientService
    {
        private readonly IEmployeeClientRepository _repository;

        public EmployeeClientService(IEmployeeClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponseDto<UserGetDto>> GetByIdAsync(int id)
        {
           
                var user = await _repository.GetByIdAsync(id);

                if (user == null)
                    return UApiResponderDto<UserGetDto>.BadRequest();

                return UApiResponderDto<UserGetDto>.Ok(user);
          
        }

        public async Task<ApiResponseDto<IEnumerable<UserGetDto>>> GetAllAsync()
        {
           
                var users = await _repository.GetAllAsync();

                var result = users.Select(u => new UserGetDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber
                });

                return UApiResponderDto<IEnumerable<UserGetDto>>.Ok(result);
          
        }

    
           
        

        public async Task<ApiResponseDto<object>> DeleteAsync(int id)
        {
            
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return UApiResponderDto<object>.BadRequest();

                await _repository.DeleteAsync(id);

                return UApiResponderDto<object>.Ok(null, "Client deleted successfully.");
          
        }
    }
}