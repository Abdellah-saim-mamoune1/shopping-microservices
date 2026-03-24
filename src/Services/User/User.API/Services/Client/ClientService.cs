using User.API.Dtos;
using User.API.Repositories.Client;
using User.API.Validators;

namespace User.API.Services.Client
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponseDto<UserGetDto>> GetByIdAsync(int id)
        {
           
                var user = await _repository.GetAsync(id);

                if (user == null)
                    return UApiResponderDto<UserGetDto>.BadRequest();

                var result = new UserGetDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber=user.PhoneNumber,
                    CreatedAt=user.CreatedAt,
                    Type=user.Type,
                };

                return UApiResponderDto<UserGetDto>.Ok(result);
         
        }

        public async Task<ApiResponseDto<object>> UpdateByIdAsync(int Id, UserUpdateDto user)
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

                await _repository.UpdateAsync(Id,user);

                return UApiResponderDto<object>.Ok(null, "Client was updated successfully.");
          
        }
    }
}