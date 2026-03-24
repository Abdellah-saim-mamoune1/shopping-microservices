using Authentication.API.Dtos;
using Authentication.API.Entities;
using Authentication.API.Repositories;
using Authentication.API.Validators;
using EventBus.Messages.Events;
using MassTransit;

namespace Authentication.API.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthenticationRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;
    public AuthenticationService(IAuthenticationRepository repository, IPublishEndpoint publishEndpoint)
    {
        _repository = repository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ApiResponseDto<AuthResponseDto>> LoginAsync(LoginDto request)
    {
        var validator = new LoginDtoValidator();
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => new ValidationErorrsDto
            {
                FieldId = e.PropertyName,
                Message = e.ErrorMessage
            }).ToList();
            return UApiResponderDto<AuthResponseDto>.BadRequest(errors);
        }

        var user = await _repository.GetUserByAccountAsync(request.Account);
        if (user == null) return UApiResponderDto<AuthResponseDto>.Unauthorized("Invalid credentials");

        var valid = _repository.VerifyPassword(user, request.Password);
        if (!valid) return UApiResponderDto<AuthResponseDto>.Unauthorized("Invalid credentials");

        var (accessToken, refreshToken, expires) = await _repository.GenerateTokensAsync(user);

        return UApiResponderDto<AuthResponseDto>.Ok(new AuthResponseDto
        {
            Id = user.Id,
            Role = user.Type,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expires
        });
    }

    public async Task<ApiResponseDto<AuthResponseDto>> RegisterClientAsync(ClientRegistrationDto request)
    {
        var validator = new ClientRegistrationDtoValidator();
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => new ValidationErorrsDto
            {
                FieldId = e.PropertyName,
                Message = e.ErrorMessage
            }).ToList();

            return UApiResponderDto<AuthResponseDto>.BadRequest(errors);
        }



        if (await _repository.ExistsByEmailAsync(request.Account))
            return UApiResponderDto<AuthResponseDto>.BadRequest(new List<ValidationErorrsDto>
            {
                new ValidationErorrsDto{
                FieldId ="Account",
                Message ="Account already exists"
                }
            });


        var user = new User { Account = request.Account, Type = "Client" };
        user = await _repository.CreateClientAsync(user, request.Password);

        var (accessToken, refreshToken, expires) = await _repository.GenerateTokensAsync(user);


        await _publishEndpoint.Publish(new UserRegistrationEvent 
        { 
          FirstName=request.FirstName,
          Account=request.Account,
          Id=user.Id,
          LastName=request.LastName,
          PhoneNumber=request.PhoneNumber,
          CreatedAt=DateTime.Now,   
          Type="Client"
        });

        return UApiResponderDto<AuthResponseDto>.Ok(new AuthResponseDto
        {
            Id = user.Id,
            Role = user.Type,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expires
        });
    }

    public async Task<ApiResponseDto<AuthResponseDto>> RegisterEmployeeAsync(EmployeeRegistrationDto request)
    {
        var validator = new EmployeeRegistrationDtoValidator();
        var validation = await validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => new ValidationErorrsDto
            {
                FieldId = e.PropertyName,
                Message = e.ErrorMessage
            }).ToList();
            return UApiResponderDto<AuthResponseDto>.BadRequest(errors);
        }

        if (await _repository.ExistsByEmailAsync(request.Account))
            return UApiResponderDto<AuthResponseDto>.BadRequest(new List<ValidationErorrsDto>
            {
                new ValidationErorrsDto{
                FieldId ="Account",
                Message ="Account already exists"
                }
            });

        var user = new User { Account = request.Account, Type = "Admin" };
        user = await _repository.CreateEmployeeAsync(user, request.Password);

        var (accessToken, refreshToken, expires) = await _repository.GenerateTokensAsync(user);

        await _publishEndpoint.Publish(new UserRegistrationEvent
        {
            FirstName = request.FirstName,
            Account = request.Account,
            Id = user.Id,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            CreatedAt = DateTime.Now,
            Type = "Admin"
        });

        return UApiResponderDto<AuthResponseDto>.Ok(new AuthResponseDto
        {
            Id = user.Id,
            Role = user.Type,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expires
        });
    }

    public async Task<ApiResponseDto<AuthResponseDto>> RefreshTokensAsync(int userId, string refreshToken)
    {
        var user = await _repository.GetUserByRefreshTokenAsync(refreshToken);
        if (user == null || user.Id != userId)
            return UApiResponderDto<AuthResponseDto>.Unauthorized("Invalid or expired refresh token");

    
        var (accessToken, newRefreshToken, expires) = await _repository.GenerateTokensAsync(user);

        return UApiResponderDto<AuthResponseDto>.Ok(new AuthResponseDto
        {
            Id = user.Id,
            Role = user.Type,
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = expires
        });
    }
}