using User.API.Dtos;
using User.API.Repositories.Employee;
using User.API.Validators;

namespace User.API.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeRepository _repository;

        public EmployeeService(EmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponseDto<UserGetDto>> GetByIdAsync(int id)
        {
            
                var user = await _repository.GetAsync(id);

                if (user == null)
                    return UApiResponderDto<UserGetDto>.BadRequest();

                return UApiResponderDto<UserGetDto>.Ok(user);
           
        }


        public async Task<ApiResponseDto<List<UserGetDto>>> GetAllAsync()
        {

            var user = await _repository.GetAllAsync();

            if (user == null)
                return UApiResponderDto<List<UserGetDto>>.BadRequest();

            return UApiResponderDto<List<UserGetDto>>.Ok(user);

        }


        public async Task<ApiResponseDto<object>> UpdateAsync(int Id, UserUpdateDto user)
        {
            
            
                List<ValidationErorrsDto> errors = new();
                var validator = new UserUpdateDtoValidator();
                var result = await validator.ValidateAsync(user);

                if (!result.IsValid)
                {
                    errors = result.Errors
                        .Select(e => new ValidationErorrsDto
                        {
                            FieldId = e.PropertyName,
                            Message = e.ErrorMessage
                        }).ToList();

                    return UApiResponderDto<object>.BadRequest(errors);
                }

                var existing = await _repository.GetAsync(Id);
                if (existing == null)
                    return UApiResponderDto<object>.BadRequest();

                await _repository.UpdateAsync(Id,user);

                return UApiResponderDto<object>.Ok(null, "Employee updated successfully.");
          
        }
    }
}